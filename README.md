# ClearMeasureTest
This is the Coding Test Homework assignment for Michael Delaney

 * CONGRATULATIONS!  At ClearMeasure, we are happy to provide you with your improved NumberGenerator Library.
 * The latest version of the libary has a number of new key features, as well as performance improvements, bug fixes,
 * and extensibility improvements.
 *
 * There are now two constructors that can be used to instantiate an instance of the NumberGenerator class:
 * 1.  new NumberGenerator(long upperBound) which instatiates a new NumberGenerator with a sequence starting at 1 and ending with upperBound
 * 2.  new NumberGenerator(long lowerBound, long upperBound) which instantiates a new NumberGenerator with a sequence starting at lowerBound and ending with upperBound
 * this allows you to specify the lower and upper bounds dynamically at runtime.
 *
 * The number generator now also allows you to introduce custom replacement functions using the .AddModularReplacement(long Divisor, string Replacement) method.
 * Once you have instantiated the NumberGenerator, you can add an arbitrary number of replacement functions using this method as follows:
 *
 * numberGenerator.AddModularReplacement(3, "Ricky") <= this will add the string "Ricky" to the return value for all numbers in the sequence that are evenly divisible by 3.
 * you can add any number of Modular replacements this way, by invoking this method multiple times.
 *
 * In addition to the ability to add in multiple modular replacement functions, we have added the ability to add in any arbitrarily complex delegate functions that take a
 * long parameter and return a bool result.  To add in a generic replacement function, you simply use: .AddFunctionReplacement(Func<long, bool> function, string replacement);
 *
 * For example:
 * .AddFunctionReplacement((x) => x > 12 && x < 20, "Teens"); <= this replaces all the numbers from 13 - 19 with the word "Teens"
 *
 * You can add any combination of modular and/or generic replacements to an instance of the NumberGenerator class, however while the class will throw an error if you attempt
 * to add duplicate modular replacements, duplicate generic replacements are allowed and will generate multiple, duplicate replacements if you are not careful not to add
 * the same generic replacement function more than once.
 *
 * The NumberGenerator class usage now requires you to fetch the next line from the generator using the .GetNextLine() method, which returns a single string as the line value
 * or in the alternative, using the .GetNextLineWords() method, which returns a List of the applicable replacement strings.  Although we have included the GetNextLine() method
 * for backwards compatibility purposes, we highly reccomend that you refactor your applications to use the .GetNextLineWords() function which will eliminate ambiguous
 * results such as:
 * "Bobby Flowers" could be a positive result for "Bobby" and "Flowers" or for "Bobby Flowers".
 *
 * COMING SOON!  Builder class for creation of NumberGenerator instances, and for simplified injection of the NumberGenerator as middleware in hosted environments.
 
