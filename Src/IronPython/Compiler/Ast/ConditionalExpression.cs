/* ****************************************************************************
 *
 * Copyright (c) Microsoft Corporation. 
 *
 * This source code is subject to terms and conditions of the Apache License, Version 2.0. A 
 * copy of the license can be found in the License.html file at the root of this distribution. If 
 * you cannot locate the  Apache License, Version 2.0, please send an email to 
 * dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
 * by the terms of the Apache License, Version 2.0.
 *
 * You must not remove this notice, or any other, from this software.
 *
 *
 * ***************************************************************************/

using MSAst = System.Linq.Expressions;

using System;

using Microsoft.Scripting.Actions;

namespace IronPython.Compiler.Ast {
    using Ast = MSAst.Expression;
    using AstUtils = Microsoft.Scripting.Ast.Utils;

    public class ConditionalExpression : Expression {
        private readonly Expression _testExpr;
        private readonly Expression _trueExpr;
        private readonly Expression _falseExpr;

        public ConditionalExpression(Expression testExpression, Expression trueExpression, Expression falseExpression) {
            _testExpr = testExpression;
            _trueExpr = trueExpression;
            _falseExpr = falseExpression;
        }

        public Expression FalseExpression {
            get { return _falseExpr; }
        }

        public Expression Test {
            get { return _testExpr; }
        }

        public Expression TrueExpression {
            get { return _trueExpr; }
        }

        public override MSAst.Expression Reduce() {
            MSAst.Expression ifTrue = AstUtils.Convert(_trueExpr, typeof(object));
            MSAst.Expression ifFalse = AstUtils.Convert(_falseExpr, typeof(object));

            return Ast.Condition(
                GlobalParent.Convert(typeof(bool), ConversionResultKind.ExplicitCast, _testExpr), 
                ifTrue, 
                ifFalse
            );
        }

        public override void Walk(PythonWalker walker) {
            if (walker.Walk(this)) {
                _testExpr?.Walk(walker);
                _trueExpr?.Walk(walker);
                _falseExpr?.Walk(walker);
            }
            walker.PostWalk(this);
        }
    }
}
