namespace ClearMeasureTestUnitTests
{
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography.X509Certificates;

    using NumberGenerator;

  
    [TestClass]
    public class UpperOnlyConstructorThrowsExceptionIfUpperLessThan1
    {
        [TestMethod]
        [DataRow(-2)]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(1)]
        public void DoesConstructorRejectInvalidUpperBoundary(long upperBound)
        {
            Assert.ThrowsException<Exception>(() => new NumberGenerator(upperBound));
        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(70134)]
        public void DoesConstructorAllowValidUpperBoundary(long upperBound)
        {
            try
            {
                var numberGenerator = new NumberGenerator(upperBound);
            }
            catch(Exception ex)
            {
                Assert.Fail("An exception was thrown for a valid upper bound");
            }

        }
    } 
    
    [TestClass]
    public class UpperAndLowerConstructorThrowsExceptionIfUpperLessThanLower
    {
        [TestMethod]
        [DataRow(5,4)]
        [DataRow(100, 90)]
        [DataRow(54982, 12398)]
        [DataRow(1, 1)]
        public void DoesConstructorRejectInvalidUpperBoundary(long lowerBound, long upperBound)
        {
            Assert.ThrowsException<Exception>(() => new NumberGenerator(lowerBound, upperBound));
        }

        [TestMethod]
        [DataRow(4,5)]
        [DataRow(300, 40004)]
        [DataRow(4,999)]
        [DataRow(-5, 0)]
        [DataRow(70134, 8823413)]
        public void DoesConstructorAllowValidUpperBoundary(long lowerBound, long upperBound)
        {
            try
            {
                var numberGenerator = new NumberGenerator(lowerBound, upperBound);
            }
            catch(Exception ex)
            {
                Assert.Fail("An exception was thrown for a valid upper bound");
            }

        }
    }

    [TestClass]
    public class ReturnsCorrectLineValues
    {

        [TestMethod]
        [DataRow(0,100)]
        [DataRow(4,50)]
        [DataRow(54982, 54987)]
        [DataRow(-1, 15)]
        public void ThrowsExceptionIfNextLineIsPulledAfterSequenceIsComplete(long lowerBound, long upperBound)
        {
            var numberGenerator = new NumberGenerator(lowerBound, upperBound);
            long testNumber = lowerBound;
            while (testNumber <= upperBound)
            {
                var line = numberGenerator.GetNextLine();
                testNumber++;
            }
            Assert.ThrowsException<Exception>(() => numberGenerator.GetNextLine());
        }

        [TestMethod]
        [DataRow(0,100)]
        [DataRow(4,50)]
        [DataRow(54982, 54987)]
        [DataRow(-1, 15)]
        public void IsCompletedReturnsTrueIfSequenceCompleted(long lowerBound, long upperBound)
        {
            var numberGenerator = new NumberGenerator(lowerBound, upperBound);
            long testNumber = lowerBound;
            while (testNumber <= upperBound)
            {
                var line = numberGenerator.GetNextLine();
                testNumber++;
            }
            Assert.IsTrue(numberGenerator.IsCompleted);
        }

        [TestMethod]
        [DataRow(0,100)]
        [DataRow(4,50)]
        [DataRow(54982, 54987)]
        [DataRow(-1, 15)]
        public void IsCompletedReturnsFalseIfSequenceNotCompleted(long lowerBound, long upperBound)
        {
            var numberGenerator = new NumberGenerator(lowerBound, upperBound);
            long testNumber = lowerBound;
            while (testNumber <= upperBound)
            {
                Assert.IsFalse(numberGenerator.IsCompleted);
                testNumber++;
            }
        }


        [TestMethod]
        [DataRow(4, 5, 3, "Ricky")]
        [DataRow(300, 40004, 5, "Bobby")]
        [DataRow(4, 999, 4, "Carol")]
        [DataRow(-5, 0, 6, "Harold")]
        [DataRow(70134, 8823413, 27, "Rumplestilskin is a jerk!")]
        public void NumberGeneratorReturnsCorrectModularReplacements(long lowerBound, long upperBound, long divisor, string replacement)
        {
            var numberGenerator = new NumberGenerator(lowerBound, upperBound);
            numberGenerator.AddModularNameReplacement(divisor, replacement);
            long testNumber = lowerBound;
            while (testNumber <= upperBound)
            {
                Assert.AreEqual(numberGenerator.GetNextLine().Contains(replacement), testNumber % divisor == 0);
                testNumber++;
            }
        }

        [TestMethod]
        [DataRow(4, 5, 3, "Ricky")]
        [DataRow(300, 40004, 5, "Bobby")]
        [DataRow(4, 999, 4, "Carol")]
        [DataRow(-5, 0, 6, "Harold")]
        [DataRow(70134, 8823413, 27, "Rumplestilskin is a jerk!")]
        public void NumberGeneratorReturnsNumberIfNoReplacementsFound(long lowerBound, long upperBound, long divisor, string replacement)
        {
            var numberGenerator = new NumberGenerator(lowerBound, upperBound);
            numberGenerator.AddModularNameReplacement(divisor, replacement);
            long testNumber = lowerBound;
            while (testNumber <= upperBound)
            {
                Assert.AreEqual(numberGenerator.GetNextLine().Contains(testNumber.ToString()), testNumber % divisor != 0);
                testNumber++;
            }
        }

        [TestMethod]
        [DataRow(-1000, 1000)]
        public void NumberGeneratorReturnsCorrectValuesForArbitraryFunctions(long lowerBound, long upperBound)
        {
            Dictionary<Func<long, bool>, string> replacements = new Dictionary<Func<long, bool>, string>();
            Func<long,bool> teen = (x) => x > 12 && x < 20;
            Func<long,bool> prime = (x) => { for (int i = 2; i <= x / 2; i++) { if (x % i == 0) { return false; } } return true; };
            Func<long,bool> positive = (x) => x > -1;
            Func<long,bool> negative = (x) => x < 0;
            string teenReplacement = "Teen";
            string primeReplacement = "Prime";
            string positiveReplacement = "Positive";
            string negativeReplacement = "Negative";

            var numberGenerator = new NumberGenerator(lowerBound, upperBound);
            numberGenerator.AddFunctionReplacement(teen, teenReplacement);
            numberGenerator.AddFunctionReplacement(prime, primeReplacement);
            numberGenerator.AddFunctionReplacement(positive, positiveReplacement);
            numberGenerator.AddFunctionReplacement(negative, negativeReplacement);
            long testNumber = lowerBound;
            while (testNumber <= upperBound)
            {
                var line = numberGenerator.GetNextLine();
                Assert.AreEqual(line.Contains(teenReplacement), teen(testNumber));
                Assert.AreEqual(line.Contains(primeReplacement), prime(testNumber));
                Assert.AreEqual(line.Contains(positiveReplacement), positive(testNumber));
                Assert.AreEqual(line.Contains(negativeReplacement), negative(testNumber));
                testNumber++;
            }
        }


        [TestMethod]
        [DataRow(4, 5, 3, "Ricky")]
        [DataRow(300, 40004, 5, "Bobby")]
        [DataRow(4, 999, 4, "Carol")]
        [DataRow(-5, 0, 6, "Harold")]
        [DataRow(70134, 8823413, 27, "Rumplestilskin is a jerk!")]
        public void NumberGeneratorThrowsErrorOnDuplicateModularReplacementFunctions(long lowerBound, long upperBound, long divisor, string replacement)
        {
            var numberGenerator = new NumberGenerator(lowerBound, upperBound);
            numberGenerator.AddModularNameReplacement(divisor, replacement);
            Assert.ThrowsException<Exception>(() => numberGenerator.AddModularNameReplacement(divisor, replacement));
        }




        [TestMethod]
        [DataRow(4, 5000, 3, "Ricky", 7, "Jim")]
        [DataRow(300, 40004, 5, "Bobby", 99, "Rupert")]
        [DataRow(4, 999, 4, "Carol", 5, "Rayne")]
        [DataRow(-5, 0, 6, "Harold", 1, "Everyman")]
        [DataRow(70134, 8823413, 27, "Rumplestilskin is a jerk!", 55, "Goldilocks has a real problem...")]
        public void NumberGeneratorReturnsMultipleReplacementsIfApplicable(long lowerBound, long upperBound, long divisor1, string replacement1, long divisor2, string replacement2)
        {
            var numberGenerator = new NumberGenerator(lowerBound, upperBound);
            numberGenerator.AddModularNameReplacement(divisor1, replacement1);
            numberGenerator.AddModularNameReplacement(divisor2, replacement2);
            long testNumber = lowerBound;
            while (testNumber <= upperBound)
            {
                string line = numberGenerator.GetNextLine();
                Assert.AreEqual(line.Contains(testNumber.ToString()), testNumber % divisor1 != 0 && testNumber % divisor2 !=0);
                Assert.AreEqual(line.Contains(replacement1), testNumber % divisor1 == 0);
                Assert.AreEqual(line.Contains(replacement2), testNumber % divisor2 == 0);
                testNumber++;
            }
        }
    }

        [TestClass]
    public class ReturnsCorrectWordValues
    {

        [TestMethod]
        [DataRow(0,100)]
        [DataRow(4,50)]
        [DataRow(54982, 54987)]
        [DataRow(-1, 15)]
        public void ThrowsExceptionIfNextLineIsPulledAfterSequenceIsComplete(long lowerBound, long upperBound)
        {
            var numberGenerator = new NumberGenerator(lowerBound, upperBound);
            long testNumber = lowerBound;
            while (testNumber <= upperBound)
            {
                var line = numberGenerator.GetNextLineWords();
                testNumber++;
            }
            Assert.ThrowsException<Exception>(() => numberGenerator.GetNextLineWords());
        }

        [TestMethod]
        [DataRow(0,100)]
        [DataRow(4,50)]
        [DataRow(54982, 54987)]
        [DataRow(-1, 15)]
        public void IsCompletedReturnsTrueIfSequenceCompleted(long lowerBound, long upperBound)
        {
            var numberGenerator = new NumberGenerator(lowerBound, upperBound);
            long testNumber = lowerBound;
            while (testNumber <= upperBound)
            {
                var line = numberGenerator.GetNextLineWords();
                testNumber++;
            }
            Assert.IsTrue(numberGenerator.IsCompleted);
        }

        [TestMethod]
        [DataRow(0,100)]
        [DataRow(4,50)]
        [DataRow(54982, 54987)]
        [DataRow(-1, 15)]
        public void IsCompletedReturnsFalseIfSequenceNotCompleted(long lowerBound, long upperBound)
        {
            var numberGenerator = new NumberGenerator(lowerBound, upperBound);
            long testNumber = lowerBound;
            while (testNumber <= upperBound)
            {
                Assert.IsFalse(numberGenerator.IsCompleted);
                testNumber++;
            }
        }


        [TestMethod]
        [DataRow(4, 5, 3, "Ricky")]
        [DataRow(300, 40004, 5, "Bobby")]
        [DataRow(4, 999, 4, "Carol")]
        [DataRow(-5, 0, 6, "Harold")]
        [DataRow(70134, 8823413, 27, "Rumplestilskin is a jerk!")]
        public void NumberGeneratorReturnsCorrectModularReplacements(long lowerBound, long upperBound, long divisor, string replacement)
        {
            var numberGenerator = new NumberGenerator(lowerBound, upperBound);
            numberGenerator.AddModularNameReplacement(divisor, replacement);
            long testNumber = lowerBound;
            while (testNumber <= upperBound)
            {
                Assert.AreEqual(numberGenerator.GetNextLineWords().Contains(replacement), testNumber % divisor == 0);
                testNumber++;
            }
        }

        [TestMethod]
        [DataRow(4, 5, 3, "Ricky")]
        [DataRow(300, 40004, 5, "Bobby")]
        [DataRow(4, 999, 4, "Carol")]
        [DataRow(-5, 0, 6, "Harold")]
        [DataRow(70134, 8823413, 27, "Rumplestilskin is a jerk!")]
        public void NumberGeneratorReturnsNumberIfNoReplacementsFound(long lowerBound, long upperBound, long divisor, string replacement)
        {
            var numberGenerator = new NumberGenerator(lowerBound, upperBound);
            numberGenerator.AddModularNameReplacement(divisor, replacement);
            long testNumber = lowerBound;
            while (testNumber <= upperBound)
            {
                Assert.AreEqual(numberGenerator.GetNextLineWords().Contains(testNumber.ToString()), testNumber % divisor != 0);
                testNumber++;
            }
        }

        [TestMethod]
        [DataRow(-1000, 1000)]
        public void NumberGeneratorReturnsCorrectValuesForArbitraryFunctions(long lowerBound, long upperBound)
        {
            Dictionary<Func<long, bool>, string> replacements = new Dictionary<Func<long, bool>, string>();
            Func<long,bool> teen = (x) => x > 12 && x < 20;
            Func<long,bool> prime = (x) => { if (x < 1) return false; for (int i = 2; i <= x / 2; i++) { if (x % i == 0) { return false; } } return true; };
            Func<long,bool> positive = (x) => x > -1;
            Func<long,bool> negative = (x) => x < 0;
            string teenReplacement = "Teen";
            string primeReplacement = "Prime";
            string positiveReplacement = "Positive";
            string negativeReplacement = "Negative";

            var numberGenerator = new NumberGenerator(lowerBound, upperBound);
            numberGenerator.AddFunctionReplacement(teen, teenReplacement);
            numberGenerator.AddFunctionReplacement(prime, primeReplacement);
            numberGenerator.AddFunctionReplacement(positive, positiveReplacement);
            numberGenerator.AddFunctionReplacement(negative, negativeReplacement);
            long testNumber = lowerBound;
            while (testNumber <= upperBound)
            {
                var words = numberGenerator.GetNextLineWords();
                Assert.AreEqual(words.Contains(teenReplacement), teen(testNumber));
                Assert.AreEqual(words.Contains(primeReplacement), prime(testNumber));
                Assert.AreEqual(words.Contains(positiveReplacement), positive(testNumber));
                Assert.AreEqual(words.Contains(negativeReplacement), negative(testNumber));
                testNumber++;
            }
        }


        [TestMethod]
        [DataRow(4, 5, 3, "Ricky")]
        [DataRow(300, 40004, 5, "Bobby")]
        [DataRow(4, 999, 4, "Carol")]
        [DataRow(-5, 0, 6, "Harold")]
        [DataRow(70134, 8823413, 27, "Rumplestilskin is a jerk!")]
        public void NumberGeneratorThrowsErrorOnDuplicateModularReplacementFunctions(long lowerBound, long upperBound, long divisor, string replacement)
        {
            var numberGenerator = new NumberGenerator(lowerBound, upperBound);
            numberGenerator.AddModularNameReplacement(divisor, replacement);
            Assert.ThrowsException<Exception>(() => numberGenerator.AddModularNameReplacement(divisor, replacement));
        }
        
        [TestMethod]
        [DataRow(4, 5000, 3, "Ricky", 7, "Jim")]
        [DataRow(300, 40004, 5, "Bobby", 99, "Rupert")]
        [DataRow(4, 999, 4, "Carol", 5, "Rayne")]
        [DataRow(-5, 0, 6, "Harold", 1, "Everyman")]
        [DataRow(70134, 8823413, 27, "Rumplestilskin is a jerk!", 55, "Goldilocks has a real problem...")]
        public void NumberGeneratorReturnsMultipleReplacementsIfApplicable(long lowerBound, long upperBound, long divisor1, string replacement1, long divisor2, string replacement2)
        {
            var numberGenerator = new NumberGenerator(lowerBound, upperBound);
            numberGenerator.AddModularNameReplacement(divisor1, replacement1);
            numberGenerator.AddModularNameReplacement(divisor2, replacement2);
            long testNumber = lowerBound;
            while (testNumber <= upperBound)
            {
                var words = numberGenerator.GetNextLineWords();
                Assert.AreEqual(words.Contains(testNumber.ToString()), testNumber % divisor1 != 0 && testNumber % divisor2 !=0);
                Assert.AreEqual(words.Contains(replacement1), testNumber % divisor1 == 0);
                Assert.AreEqual(words.Contains(replacement2), testNumber % divisor2 == 0);
                testNumber++;
            }
        }
    }


}