using IO.Swagger.Authentication;
using IO.Swagger.Controllers;
using IO.Swagger.Models;
using IO.Swagger.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Xunit;
using Assert = NUnit.Framework.Assert;


namespace CalculationNUnitTest
{

    public class Tests
    {
        private IConfigurationRoot configuration;
        private AuthService authService;
        private UserDto validUserDto = new UserDto() { UserName = "Anna", Password = "123456" };
        private UserDto invalidUserDto = new UserDto() { UserName = "Moshe", Password = "123456" };
        private UserDto invalidPasswordDto = new UserDto() { UserName = "Anna", Password = "654321" };



        [SetUp]
        public void Setup()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(TestContext.CurrentContext.TestDirectory + "..\\..\\..\\..\\..\\src\\IO.Swagger") // Set the base path to the test directory
                .AddJsonFile("appsettings.json") // Add the copied appsettings.json file
                .Build();

            authService = new AuthService(new Logger(), configuration);

            authService.Register(validUserDto);
            authService.Login(validUserDto);
        }

        protected decimal ExecAction(Func<decimal> action)
        {
            try
            {
                return action.Invoke();
            }
            catch (Exception ex)
            {
                return Decimal.MinValue;
            }
        }

        [Test]
        public void Add_TwoNumbers_ReturnsCorrectSum()
        {
            // Arrange
            CalculateBody calc = new CalculateBody()
            {Operand1 = 2, 
            Operand2 = 3,
            Operation = CalculateBody.OperationEnum.AddEnum};
            decimal expectedSum = 5;

            // Act
      
            decimal actualSum = ExecAction(() => calc.Calculate());

            // Assert
           Assert.AreEqual(expectedSum, actualSum);
        }

        [Test]
        public void Subtract_TwoNumbers_ReturnsCorrectDifference()
        {
            // Arrange
            CalculateBody calc = new CalculateBody()
            {
                Operand1 = 10,
                Operand2 = 4,
                Operation = CalculateBody.OperationEnum.SubtractEnum
            };
            decimal expectedDifference = 6;

            // Act
            decimal actualDifference = ExecAction(() => calc.Calculate());

            // Assert
            Assert.AreEqual(expectedDifference, actualDifference);
        }

        [Test]
        public void Multiply_TwoNumbers_ReturnsCorrectProduct()
        {
            // Arrange
            CalculateBody calc = new CalculateBody()
            {
                Operand1 = 7,
                Operand2 = 3,
                Operation = CalculateBody.OperationEnum.MultiplyEnum
            };

            decimal expectedProduct = 21;

            // Act
            decimal actualProduct = ExecAction(() => calc.Calculate());

            // Assert
            Assert.AreEqual(expectedProduct, actualProduct);
        }

        [Test]
        public void Divide_TwoNumbers_ReturnsCorrectQuotient()
        {
            // Arrange
            CalculateBody calc = new CalculateBody()
            {
                Operand1 = 30,
                Operand2 = 3,
                Operation = CalculateBody.OperationEnum.DivideEnum
            };

            decimal expectedQuotient = 10;

            // Act
            decimal actualQuotient = ExecAction(() => calc.Calculate());

            // Assert
            Assert.AreEqual(expectedQuotient, actualQuotient);
        }



        [Test]
        public void Divide_ByZero_ThrowsException()
        {
            // Arrange
            CalculateBody calc = new CalculateBody()
            {
                Operand1 = 10,
                Operand2 = 0,
                Operation = CalculateBody.OperationEnum.DivideEnum
            };

            // Act & Assert
            Assert.Throws<CalcException>(() => calc.Calculate());
        }

        [Test]
        public void Multiply_ByZero_ReturnsZero()
        {
            // Arrange
            CalculateBody calc = new CalculateBody()
            {
                Operand1 = 10,
                Operand2 = 0,
                Operation = CalculateBody.OperationEnum.MultiplyEnum
            };
            decimal expectedResult = 0;

            // Act
            decimal result = ExecAction(() => calc.Calculate());

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Invalid_Argument_Operator()
        {
  
            // Act
            CalculateBody calc = new CalculateBody()
            {
                Operand1 = 10,
                Operand2 = 0,
                Operation = (CalculateBody.OperationEnum)100
            };

            // Assert
            Assert.Throws<CalcException>(() => calc.Calculate());
        }

        [Test]
        public void Invalid_User()
        {
            string token = authService.Login(invalidUserDto).GetAwaiter().GetResult(); 

            // Assert
            Assert.IsNull(token);
        }

        [Test]
        public void Invalid_Password()
        {
            string token = authService.Login(invalidPasswordDto).GetAwaiter().GetResult();

            // Assert
            Assert.IsNull(token);
        }

    }
}