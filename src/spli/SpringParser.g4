parser grammar SpringParser;

options {
    tokenVocab = 'Lexer/SpringLexer';
}

parse           :   file_content;

file_content    :   scope;

scope           :   statement*;

statement       :   expression      TERMINATOR
                |   assign_var      TERMINATOR;

assign_var      :   expression ARROW DOLLAR IDENTIFIER;

array           :   LSQBR (expression (COMMA expression)*)? RSQBR;

expression      :   constant                                #ConstantExpression
                |   expression  LSQBR expression RSQBR      #IndexingExpression
                |   DOLLAR IDENTIFIER                       #IdentifierExpression;

constant        :   STRING_VALUE
                |   INTEGER_VALUE
                |   FLOAT_VALUE
                |   BOOL_VALUE
                |   NULL
                |   array;