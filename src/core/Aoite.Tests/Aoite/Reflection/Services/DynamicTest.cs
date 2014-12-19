﻿
using System;
using Aoite.Reflection;
using Xunit;
using Aoite.ReflectionTest.SampleModel.People;

namespace Aoite.ReflectionTest.Services
{
    
    public class DynamicTest
	{
		[Fact()]
		public void TestDynamicInstance()
		{
			Person original = new Person( "Bruce Lee", 25 );
			dynamic wrapper = new DynamicInstance( original );
			Assert.Equal( original.Name, wrapper.Name );
			Assert.Equal( original.Age, wrapper.Age );
			double distance;
			original.Walk( 10d, out distance );
			Assert.Equal( 10d, distance );
			wrapper.Walk( 10d, out distance );
			//Assert.Equal( 20d, distance );
			Assert.Equal( 20d, original.TryGetFieldValue( "metersTravelled" ) );
		}

		[Fact()]
		public void TestDynamicBuilder()
		{
			dynamic obj = new DynamicBuilder();
			obj.Value = 1;
			obj.GetMessage = new Func<string>( () => "Value = " + obj.Value );
			// verify
			Assert.Equal( "Value = 1", obj.GetMessage() );
			// verify that we still work after updating member
			obj.Value = 5;
			Assert.Equal( "Value = 5", obj.GetMessage() );
		}
	}
}
