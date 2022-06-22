parser grammar SpringParser;

options {
    tokenVocab = 'Lexer/SpringLexer';
}

parse           :   file_content;

file_content    :   scope;

scope           :   statement*;

statement       :   expression          TERMINATOR
                |   assign_var          TERMINATOR
                |   return_statement    TERMINATOR
                |   skip_iteration
                |   bail_statement
                |   function_def
                |   if_statement
                |   loop_statement
                |   each_loop_statement;

bail_statement  :   BAIL TERMINATOR;

skip_iteration  :   SKIP_ITERATION TERMINATOR;
            
each_loop_statement:     EACH expression DO scope END;

loop_statement  :   LOOP expression? DO scope END;

if_statement    :   IF expression DO scope (elif_statement*)? else_statement? END;

else_statement  :   ELSE DO scope;

elif_statement  :   ELIF expression DO scope;

return_statement :  RETURN expression;

assign_var      :   expression ARROW DOLLAR IDENTIFIER;

array           :   LSQBR (expression (COMMA expression)*)? RSQBR;

identifier_expr :   DOLLAR IDENTIFIER;

expression      :   constant                                    #ConstantExpression
                |   expression  LSQBR expression RSQBR          #IndexingExpression
                |   identifier_expr                             #IdentifierExpression
                |   function_call                               #FunctionCallExpression
                |   EXC_MARK expression                         #NegatedExpression
                |   expression compare_oper expression          #CompareExpression
                |   expression binary_oper expression           #BinaryExpression
                |   LPAREN expression RPAREN                    #EmphasizedExpression
                |   expression math_oper expression             #MathExpression
                |   expression TO expression (WITH expression)? #ForLoopExpression
                |   expression INSIDE expression (WITH expression)? #EachLoopExpression;

function_call   :   IDENTIFIER (expression (COMMA expression)*)?;

constant        :   STRING_VALUE
                |   INTEGER_VALUE
                |   FLOAT_VALUE
                |   BOOL_VALUE
                |   NULL
                |   array;

math_oper       :   PLUS
                |   MINUS
                |   DIV
                |   MULT
                |   POW;

compare_oper    :   EQUAL
                |   NOT_EQUAL
                |   GREATER
                |   GREATER_EQUAL
                |   LESS
                |   LESS_EQUAL;

binary_oper     :   AND 
                |   OR;

function_def    :   DEF IDENTIFIER LPAREN (IDENTIFIER (COMMA IDENTIFIER)*)? RPAREN DO scope END;