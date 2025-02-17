﻿using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Xsl;
using TcUnit.TestAdapter.Discovery;
using TcUnit.TestAdapter.Execution;
using TcUnit.TestAdapter.Models;
using TcUnit.TestAdapter.RunSettings;

namespace TcUnit.TestAdapter.Abstractions
{
    public interface ITestRunner
    {
        IEnumerable<TestCase> DiscoverTests(TwinCATXAEProject project, IMessageLogger logger);

        TestRun RunTests(TwinCATXAEProject project, IEnumerable<TestCase> tests, TestSettings runSettings, IMessageLogger logger);
    }
}
