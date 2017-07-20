﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using NUnit.Framework;
using TSQLLINT_LIB;
using TSQLLINT_LIB.Parser;
using TSQLLINT_LIB.Rules;
using TSQLLINT_LIB.Rules.RuleViolations;

namespace TSQLLINT_LIB_TESTS.Unit_Tests.Rules
{
    public class SelectStarRuleTests
    {
        private readonly TestHelper TestHelper = new TestHelper(TestContext.CurrentContext.TestDirectory);

        public static readonly object[] conditional_begin_end = {
          new object[] {"conditional-begin-end", "conditional-begin-end-no-error",  typeof(ConditionalBeginEndRule), new List<RuleViolation>()},
          new object[] {"conditional-begin-end", "conditional-begin-end-one-error", typeof(ConditionalBeginEndRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "conditional-begin-end", startLine: 1, startColumn: 1)
          }},
          new object[] {"conditional-begin-end", "conditional-begin-end-two-errors", typeof(ConditionalBeginEndRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "conditional-begin-end", startLine: 1, startColumn: 1),
              new RuleViolation(ruleName: "conditional-begin-end", startLine: 4, startColumn: 1)
          }},
          new object[] {"conditional-begin-end", "conditional-begin-end-one-error-mixed-state", typeof(ConditionalBeginEndRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "conditional-begin-end", startLine: 6, startColumn: 1)
          }}
        };

        public static readonly object[] data_compression_no_error = {
          new object[] {"data-compression", "data-compression-no-error",  typeof(DataCompressionOptionRule), new List<RuleViolation>()},
          new object[] {"data-compression", "data-compression-one-error", typeof(DataCompressionOptionRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "data-compression", startLine: 1, startColumn: 1)
          }},
          new object[] {"data-compression", "data-compression-two-errors", typeof(DataCompressionOptionRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "data-compression", startLine: 1, startColumn: 1),
              new RuleViolation(ruleName: "data-compression", startLine: 5, startColumn: 1)
          }},
          new object[] {"data-compression", "data-compression-one-error-mixed-state", typeof(DataCompressionOptionRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "data-compression", startLine: 6, startColumn: 1)
          }}
        };

        public static readonly object[] data_type_length = {
          new object[] {"data-type-length", "data-type-length-no-error",  typeof(DataTypeLengthRule), new List<RuleViolation>()},
          new object[] {"data-type-length", "data-type-length-one-error", typeof(DataTypeLengthRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "data-type-length", startLine: 3, startColumn: 19)
          }},

          new object[] {"data-type-length", "data-type-length-one-error-mixed-state", typeof(DataTypeLengthRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "data-type-length", startLine: 7, startColumn: 15)
          }},
          new object[] {"data-type-length", "data-type-length-all-errors", typeof(DataTypeLengthRule), new List<RuleViolation>
          {
            new RuleViolation(ruleName: "data-type-length", startLine: 2,  startColumn: 17),
            new RuleViolation(ruleName: "data-type-length", startLine: 3,  startColumn: 20),
            new RuleViolation(ruleName: "data-type-length", startLine: 4,  startColumn: 23),
            new RuleViolation(ruleName: "data-type-length", startLine: 5,  startColumn: 19),
            new RuleViolation(ruleName: "data-type-length", startLine: 6,  startColumn: 20),
            new RuleViolation(ruleName: "data-type-length", startLine: 7,  startColumn: 22),
            new RuleViolation(ruleName: "data-type-length", startLine: 8,  startColumn: 22),
            new RuleViolation(ruleName: "data-type-length", startLine: 9,  startColumn: 22),
            new RuleViolation(ruleName: "data-type-length", startLine: 10, startColumn: 19),
          }}
        };

        public static readonly object[] disallow_cursors = {
          new object[] {"disallow-cursors", "disallow-cursors-no-error",  typeof(DisallowCursorRule), new List<RuleViolation>()},
          new object[] {"disallow-cursors", "disallow-cursors-one-error", typeof(DisallowCursorRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "disallow-cursors", startLine: 3, startColumn: 1)
          }},
          new object[] {"disallow-cursors", "disallow-cursors-two-errors", typeof(DisallowCursorRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "disallow-cursors", startLine: 3, startColumn: 1),
              new RuleViolation(ruleName: "disallow-cursors", startLine: 4, startColumn: 1)
          }},
          new object[] {"disallow-cursors", "disallow-cursors-one-error-mixed-state", typeof(DisallowCursorRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "disallow-cursors", startLine: 4, startColumn: 1)
          }}
        };

        public static readonly object[] information_schema = {
          new object[] {"information-schema", "information-schema-no-error",  typeof(InformationSchemaRule), new List<RuleViolation>()},
          new object[] {"information-schema", "information-schema-one-error", typeof(InformationSchemaRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "information-schema", startLine: 2, startColumn: 27)
          }},
          new object[] {"information-schema", "information-schema-two-errors", typeof(InformationSchemaRule), new List<RuleViolation> 
          {
              new RuleViolation(ruleName: "information-schema", startLine: 5, startColumn: 26),
              new RuleViolation(ruleName: "information-schema", startLine: 2, startColumn: 27)
          }},
          new object[] {"information-schema", "information-schema-one-error-mixed-state", typeof(InformationSchemaRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "information-schema", startLine: 2, startColumn: 27)
          }}
        };

        public static readonly object[] object_property = {
          new object[] {"object-property", "object-property-no-error",  typeof(ObjectPropertyRule), new List<RuleViolation>()},
          new object[] {"object-property", "object-property-one-error", typeof(ObjectPropertyRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "object-property", startLine: 3, startColumn: 7)
          }},
          new object[] {"object-property", "object-property-two-errors", typeof(ObjectPropertyRule), new List<RuleViolation> 
          {
              new RuleViolation(ruleName: "object-property", startLine: 3, startColumn: 7),
              new RuleViolation(ruleName: "object-property", startLine: 8, startColumn: 7)
          }},
          new object[] {"object-property", "object-property-one-error-mixed-state", typeof(ObjectPropertyRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "object-property", startLine: 5, startColumn: 7)
          }}
        };

        public static readonly object[] print_statement = {
          new object[] {"print-statement", "print-statement-no-error",  typeof(PrintStatementRule), new List<RuleViolation>()},
          new object[] {"print-statement", "print-statement-one-error", typeof(PrintStatementRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "print-statement", startLine: 1, startColumn: 1)
          }},
          new object[] {"print-statement", "print-statement-two-errors", typeof(PrintStatementRule), new List<RuleViolation> 
          {
              new RuleViolation(ruleName: "print-statement", startLine: 1, startColumn: 1),
              new RuleViolation(ruleName: "print-statement", startLine: 2, startColumn: 1)
          }},
          new object[] {"print-statement", "print-statement-one-error-mixed-state", typeof(PrintStatementRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "print-statement", startLine: 3, startColumn: 5)
          }}
        };

        public static readonly object[] schema_qualify = {
          new object[] {"schema-qualify", "schema-qualify-no-error",  typeof(SchemaQualifyRule), new List<RuleViolation>()},
          new object[] {"schema-qualify", "schema-qualify-one-error", typeof(SchemaQualifyRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "schema-qualify", startLine: 1, startColumn: 17)
          }},
          new object[] {"schema-qualify", "schema-qualify-two-errors", typeof(SchemaQualifyRule), new List<RuleViolation> 
          {
              new RuleViolation(ruleName: "schema-qualify", startLine: 1, startColumn: 17),
              new RuleViolation(ruleName: "schema-qualify", startLine: 2, startColumn: 17)
          }},
          new object[] {"schema-qualify", "schema-qualify-one-error-mixed-state", typeof(SchemaQualifyRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "schema-qualify", startLine: 3, startColumn: 21)
          }}
        };

        public static readonly object[] select_star = {
          new object[] {"select-star", "select-star-no-error",  typeof(SelectStarRule), new List<RuleViolation>()},
          new object[] {"select-star", "select-star-one-error", typeof(SelectStarRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "select-star", startLine: 1, startColumn: 8)
          }},
          new object[] {"select-star", "select-star-two-errors", typeof(SelectStarRule), new List<RuleViolation> 
          {
              new RuleViolation(ruleName: "select-star", startLine: 1, startColumn: 8),
              new RuleViolation(ruleName: "select-star", startLine: 3, startColumn: 14)
          }},
          new object[] {"select-star", "select-star-one-error-mixed-state", typeof(SelectStarRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "select-star", startLine: 3, startColumn: 12)
          }}
        };

        public static readonly object[] semicolon_termination = {
          new object[] {"semicolon-termination", "semicolon-termination-no-error",  typeof(SemicolonTerminationRule), new List<RuleViolation>()},
          new object[] {"semicolon-termination", "semicolon-termination-one-error", typeof(SemicolonTerminationRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "semicolon-termination", startLine: 1, startColumn: 18)
          }},
          new object[] {"semicolon-termination", "semicolon-termination-one-error-with-tab", typeof(SemicolonTerminationRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "semicolon-termination", startLine: 2, startColumn: 21)
          }},
          new object[] {"semicolon-termination", "semicolon-termination-multiple-errors", typeof(SemicolonTerminationRule), new List<RuleViolation> 
          {
              new RuleViolation(ruleName: "semicolon-termination", startLine: 1, startColumn: 20),
              new RuleViolation(ruleName: "semicolon-termination", startLine: 4, startColumn: 13),
              new RuleViolation(ruleName: "semicolon-termination", startLine: 12, startColumn: 47),
              new RuleViolation(ruleName: "semicolon-termination", startLine: 14, startColumn: 29),
              new RuleViolation(ruleName: "semicolon-termination", startLine: 20, startColumn: 47),

          }},
          new object[] {"semicolon-termination", "semicolon-termination-one-error-mixed-state", typeof(SemicolonTerminationRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "semicolon-termination", startLine: 1, startColumn: 20)
          }},
          new object[] {"semicolon-termination", "semicolon-termination-try-catch-while",  typeof(SemicolonTerminationRule), new List<RuleViolation>()},
        };

        public static readonly object[] set_ansi = {
          new object[] {"set-ansi", "set-ansi-no-error",  typeof(SetAnsiNullsRule), new List<RuleViolation>()},
          new object[] {"set-ansi", "set-ansi-one-error", typeof(SetAnsiNullsRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "set-ansi", startLine: 1, startColumn: 1)
          }}
        };

        public static readonly object[] set_nocount = {
          new object[] {"set-nocount", "set-nocount-no-error",  typeof(SetNoCountRule), new List<RuleViolation>()},
          new object[] {"set-nocount", "set-nocount-one-error", typeof(SetNoCountRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "set-nocount", startLine: 1, startColumn: 1)
          }},
          new object[] {"set-nocount", "set-nocount-no-rowset-action",  typeof(SetNoCountRule), new List<RuleViolation>()},
        };

        public static readonly object[] set_quoted_identifier = {
          new object[] {"set-quoted-identifier", "set-quoted-identifier-no-error",  typeof(SetQuotedIdentifierRule), new List<RuleViolation>()},
          new object[] {"set-quoted-identifier", "set-quoted-identifier-one-error", typeof(SetQuotedIdentifierRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "set-quoted-identifier", startLine: 1, startColumn: 1)
          }}
        };

        public static readonly object[] set_transaction_isolation_level = {
          new object[] {"set-transaction-isolation-level", "set-transaction-isolation-level-no-error",  typeof(SetTransactionIsolationLevelRule), new List<RuleViolation>()},
          new object[] {"set-transaction-isolation-level", "set-transaction-isolation-level-one-error", typeof(SetTransactionIsolationLevelRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "set-transaction-isolation-level", startLine: 1, startColumn: 1)
          }}
        };

        public static readonly object[] upper_lower = {
          new object[] {"upper-lower", "upper-lower-no-error",  typeof(UpperLowerRule), new List<RuleViolation>()},
          new object[] {"upper-lower", "upper-lower-one-error", typeof(UpperLowerRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "upper-lower", startLine: 1, startColumn: 8)
          }},
          new object[] {"upper-lower", "upper-lower-two-errors", typeof(UpperLowerRule), new List<RuleViolation> 
          {
              new RuleViolation(ruleName: "upper-lower", startLine: 1, startColumn: 8),
              new RuleViolation(ruleName: "upper-lower", startLine: 2, startColumn: 8)
          }},
          new object[] {"upper-lower", "upper-lower-one-error-mixed-state", typeof(UpperLowerRule), new List<RuleViolation>
          {
              new RuleViolation(ruleName: "upper-lower", startLine: 3, startColumn: 8)
          }}
        };

        [Test, TestCaseSource("conditional_begin_end"), 
               TestCaseSource("data_compression_no_error"),
               TestCaseSource("data_type_length"),
               TestCaseSource("disallow_cursors"),
               TestCaseSource("information_schema"),
               TestCaseSource("object_property"),
               TestCaseSource("print_statement"),
               TestCaseSource("schema_qualify"),
               TestCaseSource("select_star"),
               TestCaseSource("semicolon_termination"),
               TestCaseSource("set_ansi"),
               TestCaseSource("set_nocount"),
               TestCaseSource("set_quoted_identifier"),
               TestCaseSource("set_transaction_isolation_level"),
               TestCaseSource("upper_lower")]
        public void RuleTests(string rule, string testFileName, Type ruleType, List<RuleViolation> expectedRuleViolations)
        {
            // arrange
            var sqlString = TestHelper.GetTestFile(string.Format("Rules\\{0}\\{1}.sql", rule, testFileName));

            var fragmentVisitor = new SqlRuleVisitor(null);
            var ruleViolations = new List<RuleViolation>();

            Action <string, string, int, int> ErrorCallback = delegate (string ruleName, string ruleText, int startLine, int startColumn)
            {
                ruleViolations.Add(new RuleViolation(ruleName, startLine, startColumn));
            };

            var visitor = (TSqlFragmentVisitor)Activator.CreateInstance(ruleType, ErrorCallback);
            var textReader = Utility.CreateTextReaderFromString(sqlString);

            // act
            fragmentVisitor.VisistRule(textReader, visitor);

            ruleViolations = ruleViolations.OrderBy(o => o.Line).ToList();
            expectedRuleViolations = expectedRuleViolations.OrderBy(o => o.Line).ToList();

            // assert
            Assert.AreEqual(expectedRuleViolations.Count, ruleViolations.Count);

            var compareer = new RuleViolationComparer();
            CollectionAssert.AreEqual(expectedRuleViolations, ruleViolations, compareer);
        }
        
        public class RuleViolationComparer : IComparer, IComparer<RuleViolation>
        {
            public int Compare(object x, object y)
            {
                var lhs = x as RuleViolation;
                var rhs = y as RuleViolation;
                if (lhs == null || rhs == null) throw new InvalidOperationException();
                return Compare(lhs, rhs);
            }

            public int Compare(RuleViolation x, RuleViolation y)
            {
                if (x.Line != y.Line)
                {
                    return -1;
                }
                if (x.Column != y.Column)
                {
                    return -1;
                }
                if (x.RuleName != y.RuleName)
                {
                    return -1;
                }

                return 0;
            }
        }
    }
}
