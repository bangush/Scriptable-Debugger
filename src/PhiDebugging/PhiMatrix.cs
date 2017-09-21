using System;
using System.Collections.Generic;

namespace PhiDebugging
{
    public class PhiMatrix
    {
        public PhiMatrix()
        {
            Matrix = new Dictionary<Tuple<TestAction, TestResult>, IntCount>()
            {
                {Tuple.Create(TestAction.Covered, TestResult.Fail), new IntCount(0) }, {Tuple.Create(TestAction.Covered, TestResult.Pass), new IntCount(0) },
                {Tuple.Create(TestAction.Uncovered, TestResult.Fail), new IntCount(0) }, {Tuple.Create(TestAction.Uncovered, TestResult.Pass), new IntCount(0) }
            };
        }

        public Dictionary<Tuple<TestAction, TestResult>, IntCount> Matrix { get; }

        public double CalculatePhi()
        {
            var n_11 = Matrix[Tuple.Create(TestAction.Covered, TestResult.Fail)].Value;
            var n_10 = Matrix[Tuple.Create(TestAction.Covered, TestResult.Pass)].Value;

            var n_01 = Matrix[Tuple.Create(TestAction.Uncovered, TestResult.Fail)].Value;
            var n_00 = Matrix[Tuple.Create(TestAction.Uncovered, TestResult.Pass)].Value;

            var n_1o = n_11 + n_10;
            var n_0o = n_01 + n_00;

            var n_o1 = n_11 + n_01;
            var n_o0 = n_10 + n_00;

            var phi = ((n_11 * n_00) - (n_10 * n_01)) / Math.Sqrt(n_1o * n_0o * n_o1 * n_o0);

            return phi;
        }

        public void Increment(TestAction action, TestResult result)
        {
            Matrix[Tuple.Create(action, result)] = Matrix[Tuple.Create(action, result)].Increment();
        }

        public static PhiMatrix Create()
        {
            return new PhiMatrix();
        }
    }
}