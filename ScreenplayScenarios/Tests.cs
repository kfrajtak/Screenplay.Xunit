using CSF.Screenplay;
using static CSF.Screenplay.StepComposer;
using OpenQA.Selenium;
using FluentAssertions;
using Xunit;
using System.Collections.Generic;
using Xunit.Abstractions;
using System.Linq;
using Xunit.Sdk;
using System;
using System.Reflection;
using Screenplay.XUnit;
using CSF.Screenplay.Abilities;

namespace ScreenplayScenarios
{
    public class Tests
    {
        [Fact]
        public void Working() { }

        [Screenplay]
        public void NotWorking() { }

        [Screenplay]
        public void NotWorkingToo() { }

        [Fact]
        public void WorkingToo()
        {
        }

        [Screenplay]
        public void NotWorkingWithParams(ICast cast, MyAbility ability)
        {
            Assert.False(true);
        }

        [Screenplay]
        public void AGAG(ICast cast, MyAbility ability)
        {
        }

        [Fact]
        public void X3() { }

        [Fact]
        public void X4() { X(); }

        [Screenplay]
        public void X()
        {
            
        }

        [Screenplay]
        public void XX(ICast cast, IAbility ability)
        {
       
        }
    }
}
