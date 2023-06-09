﻿// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
namespace SpreadsheetUtilities
{
    /// <summary>
    /// Author: Mason Sansom
    /// Partner: -none-
    /// Date: 25-Jan-2023
    /// Course:    CS 3500, University of Utah, School of Computing
    /// Copyright: CS 3500 and Mason Sansom - This work may not 
    ///            be copied for use in Academic Coursework.
    ///
    /// I, Mason Sansom, certify that I wrote this code from the skeleton implementation 
    /// provided and
    /// All references used in the completion of the assignments are cited 
    /// in my README file.
    ///
    /// File Contents
    /// 
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG iscalled dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG iscalled dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        private Dictionary<String, HashSet<string>> dependents;
        private Dictionary<String, HashSet<string>> dependees;
        private int size;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new Dictionary<String, HashSet<string>>();
            dependees = new Dictionary<String, HashSet<string>>();
            size = 0;
        }
        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return size; }
        }
        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, youwould
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
            {
                get
                {
                if (!dependees.ContainsKey(s))
                    return 0;
                return dependees[s].Count(); 
                }
            }
        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            if(!dependents.ContainsKey(s)) 
                return false;
            return dependents[s].Count() != 0;
        }
        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            if(!dependees.ContainsKey(s))
                return false;
            return dependees[s].Count() != 0;
        }
        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (!dependents.ContainsKey(s) || dependents[s].Count == 0)
                return new HashSet<string>();
            return dependents[s];
        }
        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (!dependees.ContainsKey(s) || dependees[s].Count == 0)
                return new HashSet<string>();
            return dependees[s];
        }
        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            if(dependents.ContainsKey(s))
            {
                if (!dependents[s].Contains(t))
                {
                    dependents[s].Add(t);
                    size++;
                }
            } else
            {
                dependents.Add(s, new HashSet<string> { t });
                size++;
            }

            if(dependees.ContainsKey(t))
            {
                if (!dependees[t].Contains(s))
                {
                    dependees[t].Add(s);
                }
            }
            else
            {
                dependees.Add(t, new HashSet<string> { s });
            }

        }
        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            if (dependents.ContainsKey(s))
            {
                if (dependents[s].Count() == 1 && dependents[s].Contains(t))
                {
                    dependents.Remove(s);
                    size--;
                }
                else
                {
                    dependents[s].Remove(t);
                    size--;
                }

            }

            if (dependees.ContainsKey(t))
            {
                if (dependees[t].Count() == 1 && dependees[t].Contains(s))
                {
                    dependees.Remove(t);
                }
                else
                {
                    dependees[t].Remove(s);
                }

            }
        }
        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (dependents.ContainsKey(s))
            {
                foreach (string r in dependents[s])
                {
                    if (dependees[r].Count == 1)
                    {
                        dependees.Remove(r);
                    }
                    else
                    {
                        dependees[r].Remove(s);
                    }
                    size--;
                }
                dependents[s].Clear();
            }

            

            foreach (string t in newDependents)
            {
                this.AddDependency(s, t);
            }
        }
        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            if (dependees.ContainsKey(s))
            {
                foreach (string r in dependees[s])
                {
                    if (dependents[r].Count == 1)
                    {
                        dependents.Remove(r);
                    }
                    else
                    {
                        dependents[r].Remove(s);
                    }
                    size--;
                }
                dependees[s].Clear();
            }

            

            foreach (string t in newDependees)
            {
                this.AddDependency(t, s);
            }
        }
    }
}
