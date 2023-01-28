using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
namespace DevelopmentTests
{
    /// <summary>
    ///This is a test class for DependencyGraphTest and is intended
    ///to contain all DependencyGraphTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DependencyGraphTest
    {
        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }
        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }
        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyEnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            IEnumerator<string> e1 = t.GetDependees("y").GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("x", e1.Current);
            IEnumerator<string> e2 = t.GetDependents("x").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            Assert.AreEqual("y", e2.Current);
            t.RemoveDependency("x", "y");
            Assert.IsFalse(t.GetDependees("y").GetEnumerator().MoveNext());
            Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
        }
        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void SimpleReplaceTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.RemoveDependency("x", "y");
            t.ReplaceDependents("x", new HashSet<string>());
            t.ReplaceDependees("y", new HashSet<string>());
        }
        ///<summary>
        ///It should be possibe to have more than one DG at a time.
        ///</summary>
        [TestMethod()]
        public void StaticTest()
        {
            DependencyGraph t1 = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph();
            t1.AddDependency("x", "y");
            Assert.AreEqual(1, t1.Size);
            Assert.AreEqual(0, t2.Size);
        }
        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
        }
        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void EnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));
            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }
        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void ReplaceThenEnumerate()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            t.AddDependency("a", "z");
            t.ReplaceDependents("b", new HashSet<string>());
            t.AddDependency("y", "b");
            t.ReplaceDependents("a", new HashSet<string>() { "c" });
            t.AddDependency("w", "d");
            t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
            t.ReplaceDependees("d", new HashSet<string>() { "b" });
            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));
            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }
        /// <summary>
        ///Using lots of data
        ///</summary>
        [TestMethod()]
        public void StressTest()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();
            // A bunch of strings to use
            const int SIZE = 200;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }
            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }
            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }
            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 4; j < SIZE; j += 4)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }
            // Add some back
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j += 2)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }
            // Remove some more
            for (int i = 0; i < SIZE; i += 2)
            {
                for (int j = i + 3; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }
            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new
        HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new
        HashSet<string>(t.GetDependees(letters[i]))));
            }
        }
        /// <summary>
        /// Testing if the x being looked for
        /// doesn't exist
        /// </summary>
        [TestMethod()]
        public void HasDependentsDoesntExist()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependents("s"));
        }
        /// <summary>
        /// Tests if an item has dependents
        /// </summary>
        [TestMethod()]
        public void HasDependents()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("t", "s");
            Assert.IsTrue(t.HasDependents("t"));
        }
        /// <summary>
        /// Tests if a given object doesnt have dependents
        /// </summary>
        [TestMethod()]
        public void DoesntHaveDependents()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("t", "s");
            Assert.IsFalse(t.HasDependents("s"));
        }
        /// <summary>
        /// Checks if the given object
        /// doesnt exist 
        /// </summary>
        [TestMethod()]
        public void HasDependeesDoesntExist()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependees("s"));
        }
        /// <summary>
        /// Checks if the given object has dependees
        /// </summary>
        [TestMethod()]
        public void HasDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("t", "s");
            Assert.IsTrue(t.HasDependees("s"));
        }
        /// <summary>
        /// Checks if the given object doesnt
        /// have dependees
        /// </summary>
        [TestMethod()]
        public void DoesntHaveDependees()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("t", "s");
            Assert.IsFalse(t.HasDependees("t"));
        }
        /// <summary>
        /// Tests replacing dependees
        /// </summary>
        [TestMethod()]
        public void ReplaceDependees()
        {
            DependencyGraph t = new DependencyGraph();
            List<String> dependees = new List<string>{"w", "h" };
            t.AddDependency("t", "s");
            t.ReplaceDependees("s", dependees);
            Assert.IsTrue(t.GetDependees("s").Contains("w"));
            Assert.IsTrue(t.GetDependees("s").Contains("h"));
            Assert.IsFalse(t.GetDependees("s").Contains("t"));
            Assert.IsTrue(t.GetDependents("w").Contains("s"));
            Assert.IsTrue(t.GetDependents("h").Contains("s"));
            Assert.IsFalse(t.HasDependents("t"));
        }
        /// <summary>
        /// Checks an edge case of replacing 
        /// Dependees
        /// </summary>
        [TestMethod()]
        public void ReplaceManyDependees()
        {
            DependencyGraph t = new DependencyGraph();
            List<String> dependees = new List<string> { "w", "h" };
            t.AddDependency("t", "s");
            t.AddDependency("t", "x");
            t.ReplaceDependees("s", dependees);
            Assert.IsTrue(t.GetDependees("s").Contains("w"));
            Assert.IsTrue(t.GetDependees("s").Contains("h"));
            Assert.IsFalse(t.GetDependees("s").Contains("t"));
            Assert.IsTrue(t.GetDependents("w").Contains("s"));
            Assert.IsTrue(t.GetDependents("h").Contains("s"));
            Assert.IsTrue(t.GetDependents("t").Contains("x"));
            Assert.IsFalse(t.GetDependents("t").Contains("s"));
        }
        /// <summary>
        /// Tests replacing dependents
        /// </summary>
        [TestMethod()]
        public void ReplaceDependents()
        {
            DependencyGraph t = new DependencyGraph();
            List<String> dependents = new List<string> { "w", "h" };
            t.AddDependency("t", "s");
            t.ReplaceDependents("t", dependents);
            Assert.IsTrue(t.GetDependents("t").Contains("w"));
            Assert.IsTrue(t.GetDependents("t").Contains("h"));
            Assert.IsFalse(t.GetDependents("t").Contains("t"));
            Assert.IsTrue(t.GetDependees("w").Contains("t"));
            Assert.IsTrue(t.GetDependees("h").Contains("t"));
            Assert.IsFalse(t.HasDependents("s"));
        }
        /// <summary>
        /// Tests an edge case of replacing dependees
        /// </summary>
        [TestMethod()]
        public void ReplaceManyDependents()
        {
            DependencyGraph t = new DependencyGraph();
            List<String> dependents = new List<string> { "w", "h" };
            t.AddDependency("t", "s");
            t.AddDependency("x", "s");
            t.ReplaceDependents("t", dependents);
            Assert.IsTrue(t.GetDependents("t").Contains("w"));
            Assert.IsTrue(t.GetDependents("t").Contains("h"));
            Assert.IsFalse(t.GetDependents("t").Contains("t"));
            Assert.IsTrue(t.GetDependees("w").Contains("t"));
            Assert.IsTrue(t.GetDependees("h").Contains("t"));
            Assert.IsFalse(t.HasDependents("s"));
            Assert.IsTrue(t.GetDependees("s").Contains("x"));
            Assert.IsFalse(t.GetDependees("s").Contains("t"));
        }
    }
}