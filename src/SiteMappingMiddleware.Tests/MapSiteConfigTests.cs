﻿namespace SiteMappingMiddleware
{
    using FluentAssertions;
    using Xunit;

    public class MapSiteConfigTests
    {
        private readonly MapSiteConfig _config1;
        private readonly MapSiteConfig _sameAsConfig1;
        private readonly MapSiteConfig _differentToConfig1;

        public MapSiteConfigTests()
        {
            _config1 = new MapSiteConfig("host1");
            _sameAsConfig1 = new MapSiteConfig("host1");
            _differentToConfig1 = new MapSiteConfig("host2");
        }


        [Fact]
        public void When_same_then_equals_should_be_true()
        {
            _config1.Equals(_sameAsConfig1).Should().BeTrue();
        }

        [Fact]
        public void When_same_then_operator_equals_should_be_true()
        {
            (_config1 == _sameAsConfig1).Should().BeTrue();
        }

        [Fact]
        public void When_same_then_operator_does_not_equal_should_be_false()
        {
            (_config1 != _sameAsConfig1).Should().BeFalse();
        }

        [Fact]
        public void When_same_instance_then_equals_should_be_true()
        {
            _config1.Equals(_config1).Should().BeTrue();
        }

        [Fact]
        public void When_same_as_object_then_equals_should_be_true()
        {
            _config1.Equals((object)_sameAsConfig1).Should().BeTrue();
        }

        [Fact]
        public void When_diffent_then_equals_should_be_false()
        {
            _config1.Equals(_differentToConfig1).Should().BeFalse();
        }

        [Fact]
        public void When_different_then_operator_equals_should_be_false()
        {
            (_config1 == _differentToConfig1).Should().BeFalse();
        }

        [Fact]
        public void When_different_then_operator_does_not_equal_should_be_true()
        {
            (_config1 != _differentToConfig1).Should().BeTrue();
        }

        [Fact]
        public void When_diffent_as_object_then_equals_should_be_false()
        {
            _config1.Equals((object)_differentToConfig1).Should().BeFalse();
        }

        [Fact]
        public void When_diffent_instance_then_equals_should_be_false()
        {
            _config1.Equals((object)_differentToConfig1).Should().BeFalse();
        }
    }
}