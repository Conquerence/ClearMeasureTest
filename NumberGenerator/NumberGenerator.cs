namespace NumberGenerator
{
    public class NumberGenerator
    {
        private long lowerBound;
        private long upperBound;
        private long currentValue;

        private List<long> modularReplacements = new List<long>();

        private Dictionary<Func<long,bool>, string> ReplacementFunctions;

        private List<string> GetLineWords(long number)
        {
            List<string> result = new List<string>();

            // Cycle through every function in the list of functions
            foreach (Func<long, bool> function in this.ReplacementFunctions.Keys)
            {
                if (function(number))  // <= test the number with the function
                    result.Add(this.ReplacementFunctions[function]); // <= if the function returns true, add the replacement string to the list.
            }
            if (result.Count == 0) // <= if there are no replacement strings in the list
                result.Add(number.ToString()); // <= add the number to the list as a string.
            return result;
        }

        /// <summary>
        /// Returns true if the number generator has finished processing all elements in the sequence.
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                return this.currentValue > this.upperBound;
            } 
        }

        /// <summary>
        /// Returns the currentValue of the sequence
        /// </summary>
        public long CurrentValue
        {
            get
            {
                return currentValue;
            }
        }

        /// <summary>
        /// Returns the next number result as a list of strings.
        /// </summary>
        /// <returns>A list of the replacement strings that apply to the next number in the sequence, or the number as a string if no replacements are applicable.</returns>
        /// <exception cref="Exception">Throws and exception if the end of the sequence has been reached</exception>
        public List<string> GetNextLineWords()
        {
            if (IsCompleted)
                throw new Exception("End of Sequence Reached.");
            var result = GetLineWords(this.currentValue);
            this.currentValue++;
            return result;
        }

        /// <summary>
        /// Returns the next number result as a single string line.
        /// </summary>
        /// <returns>A string line that represents the results of the next number in the sequence.</returns>
        /// <exception cref="Exception">Throws and exception if the end of the sequence has been reached</exception>
        public string GetNextLine()
        {
            string result = "";
            foreach (var word in GetNextLineWords())
            {
                result += $"{word} ";
            }
            return result.Trim();
        }

        /// <summary>
        /// Adds a divisor replacement.  If the given number is evenly divisible by the provided divisor, then the replacement string is added to the line value.
        /// </summary>
        /// <param name="Divisor"></param>
        /// <param name="Replacement"></param>
        /// <exception cref="Exception">Throws and exception if the function has already been added.  Each function can have only one replacement string</exception>
        public void AddModularNameReplacement(long Divisor, string Replacement)
        {
            if (this.modularReplacements.Contains(Divisor))
                throw new Exception("Duplicate Function Added!");
            this.modularReplacements.Add(Divisor);
            Func<long, bool> function = (x) => x % Divisor == 0;
            AddFunctionReplacement(function, Replacement);
        }

        /// <summary>
        /// Adds a replacement function to the number generator.
        /// </summary>
        /// <param name="Function">A Func<long,bool> function that returns true or false based on the long number.  If the function returns true for a given long, then the replacement string will be added to the line value.</long></param>
        /// <param name="Replacement">The replacement string to use if the function returns true</param>
        public void AddFunctionReplacement(Func<long, bool> Function, string Replacement)
        {
            this.ReplacementFunctions.Add(Function, Replacement);
        }

        /// <summary>
        /// Creates a new number generator class with a lower bound of 1 and an upper bound supplied by the UpperBound parameter
        /// </summary>
        /// <param name="LowerBound">The lower bound of the number generator</param>
        /// <param name="UpperBound">The upper bound of the number generator</param>
        public NumberGenerator(long LowerBound, long UpperBound)
        {
            Initialize(LowerBound, UpperBound);
        }

        /// <summary>
        /// Creates a new number generator class with a lower bound of 1 and an upper bound supplied by the UpperBound parameter
        /// </summary>
        /// <param name="UpperBound">The upper bound of the number generator</param>
        public NumberGenerator(long UpperBound)
        {
            Initialize(1, UpperBound);
        }

        private void Initialize(long LowerBound, long UpperBound)
        {
            if(LowerBound >= UpperBound)
                throw new Exception($"Upperbound ({UpperBound}) must be greater than LowerBound ({LowerBound}) !");
            this.lowerBound = LowerBound;
            this.upperBound = UpperBound;
            currentValue = LowerBound;
            this.ReplacementFunctions = new Dictionary<Func<long, bool>, string>();
        }
    
    }
}