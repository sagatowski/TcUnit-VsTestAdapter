﻿using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TcUnit.TestAdapter.Abstractions;
using TcUnit.TestAdapter.Common;
using TcUnit.TestAdapter.Discovery;
using TcUnit.TestAdapter.Models;
using TcUnit.TestAdapter.RunSettings;
using TwinCAT.Ads;
using static System.Net.Mime.MediaTypeNames;

namespace TcUnit.TestAdapter.Execution
{
    public class TestRunner : ITestRunner
    {
        private readonly XUnitTestResultParser testResultParser = new XUnitTestResultParser();

        public IEnumerable<TestCase> DiscoverTests(TwinCATXAEProject project, IMessageLogger logger)
        {
            var tests = new List<TestCase>();

            foreach (var plcProject in project.PlcProjects)
            {
                if(!IsPlcProjectSuitableForTestRun(plcProject))
                {
                    logger.SendMessage(TestMessageLevel.Informational, "Found non suitable PLC project");
                    continue;
                }

                // find possible test suites and test cases
                var testCases = plcProject.FunctionBlocks
                        .Where(pou => pou.Extends == TestAdapter.TestSuiteBaseClass);

                foreach (var pou in testCases)
                {
                    var testSuite = TestSuite.ParseFromFunctionBlock(pou);

                    foreach (var testMethod in testSuite.Tests)
                    {
                        var testName = plcProject.Name + "." + testMethod.Name;

                        var test = new TestCase(testName, TestAdapter.ExecutorUri, project.FilePath);
                        test.LineNumber = 0;
                        test.CodeFilePath = pou.FilePath;
                        test.DisplayName = testName;

                        tests.Add(test);
                    }
                }
            }

            return tests;
        }

        public TestRun RunTests(TwinCATXAEProject project, IEnumerable<TestCase> tests, TestSettings runSettings, IMessageLogger logger)
        {

            if(!project.IsProjectPreBuild)
            {
                throw new Exception("TwinCAT XAE project is not pre build.");
            }

            if (!project.IsPlcProjectIncluded)
            {
                throw new Exception("TwinCAT XAE project doas not contain at least one PLC projects.");
            }

            var target = runSettings.Target;
            var cleanUpAfterTestRun = runSettings.CleanUpAfterTestRun;
            var cleanUpBeforeTestRun = true;
           
            var targetRuntime = new TargetRuntime(target);

            if(!targetRuntime.IsReachable)
            {
                throw new Exception("Target is not connected");
            }

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            PrepareTargetForTestRun(targetRuntime, project, cleanUpBeforeTestRun);

            PerformTestRunOnTarget(targetRuntime);

            var testResults = CollectTestRunResultsFromTarget(targetRuntime);

            if (cleanUpAfterTestRun)
            {
                CleanUpTargetAfterTestRun(targetRuntime);
            }
            
            stopWatch.Stop();

            var testRunDuration = stopWatch.Elapsed;

            var testRunResults = PopulateTestRunResults(tests, testResults);


            var testRun = new TestRun
            {
                Results = testRunResults,
                Conditions = new TestRunConditions
                {
                    TwinCATVersion = targetRuntime.Info.TwinCATVersion.ToString(),
                    Target = target,
                    OperatingSystem = targetRuntime.Info.ImageOsName,
                    Duration = testRunDuration,
                    BuildConfiguration = RTOperatingSystem.GetBuildConfigurationFromRTPlatform( targetRuntime.Info.RTPlatform )
                }
            };   

            return testRun;
        }

        private bool IsPlcProjectSuitableForTestRun (PlcProject project)
        {
            var tcUnitLibraryReference = project.References.Where(r => r.Name == "TcUnit");

            if (!tcUnitLibraryReference.Any())
            {
                return false;
            }

            var library = tcUnitLibraryReference.FirstOrDefault();

            var xUnitEnablePublish = "";

            if (!library.Parameters.TryGetValue("XUNITENABLEPUBLISH", out xUnitEnablePublish))
            {
                return false;
            }

            if(string.IsNullOrEmpty(xUnitEnablePublish) || !xUnitEnablePublish.Equals("TRUE"))
            {
                return false;
            }

            var xUnitFilePath = "";

            if (!library.Parameters.TryGetValue("XUNITFILEPATH", out xUnitFilePath))
            {
                return false;
            }

            if (string.IsNullOrEmpty(xUnitFilePath) )
            {
                return false;
            }

            var useDataTypeAsTestNames = "";

            if (!library.Parameters.TryGetValue("USEDATATYPESASTESTNAMES", out useDataTypeAsTestNames))
            {
                return false;
            }

            if (string.IsNullOrEmpty(useDataTypeAsTestNames) || !useDataTypeAsTestNames.Equals("TRUE"))
            {
                return false;
            }

            return true;
        }

        private void PrepareTargetForTestRun (TargetRuntime target, TwinCATXAEProject project, bool cleanUpBeforeTestRun)
        {
            target.SwitchToConfigMode();
            target.DownloadProject(project, cleanUpBeforeTestRun);
        }


        private IEnumerable<TestResult> PopulateTestRunResults (IEnumerable<TestCase> testCases, IEnumerable<TestCaseResult> testResults)
        {
            var testRunResults = new List<TestResult>();

            foreach (var test in testCases)
            {
                var testResult = new TestResult(test);

                var result = testResults.Where(r => r.FullyQualifiedName == test.FullyQualifiedName).First();

                if (result != null)
                {
                    testResult.Outcome = result.Outcome;
                    testResult.Duration = result.Duration;
                    testResult.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    testResult.Outcome = TestOutcome.Skipped;
                }

                testRunResults.Add(testResult);
            }

            return testRunResults;
        }
        private void CleanUpTargetAfterTestRun (TargetRuntime target)
        {
            target.SwitchToConfigMode();
            target.CleanUpBootFolder();
        }

        private void PerformTestRunOnTarget(TargetRuntime target)
        {
            target.SwitchToRunMode();

            var isTestRunFinished = false;

            while (!isTestRunFinished)
            {
                isTestRunFinished = target.IsTestRunFinished();
                Thread.Sleep(500);
            }
        }

        private IEnumerable<TestCaseResult> CollectTestRunResultsFromTarget (TargetRuntime target)
        {
            target.UploadTestRunResults(@"C:\Temp\tcunit_testresults.xml");
            return testResultParser.ParseFromFile(@"C:\Temp\testresults.xml");
        }
    }
}
