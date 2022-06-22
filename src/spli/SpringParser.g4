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
                |   function_def;

return_statement :  RETURN expression;

assign_var      :   expression ARROW DOLLAR IDENTIFIER;

array           :   LSQBR (expression (COMMA expression)*)? RSQBR;

expression      :   constant                                #ConstantExpression
                |   expression  LSQBR expression RSQBR      #IndexingExpression
                |   DOLLAR IDENTIFIER                       #IdentifierExpression
                |   function_call                           #FunctionCallExpression;

function_call   :   IDENTIFIER DOT (expression (COMMA expression)*)?;

constant        :   STRING_VALUE
                |   INTEGER_VALUE
                |   FLOAT_VALUE
                |   BOOL_VALUE
                |   NULL
                |   array;

function_def    :   DEF IDENTIFIER LPAREN (IDENTIFIER (COMMA IDENTIFIER)*)? RPAREN DO scope END;