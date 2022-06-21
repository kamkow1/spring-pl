parser grammar SpringParser;

options {
    tokenVocab = './Lexer/SpringLexer';
}

parse           :   file_content;

file_content    :   scope;

scope           :   statement*;

statement       :   expression
                |   assign_var;

assign_var      :   expression ARROW IDENTIFIER;

expression      :   constant;

constant        :   STRING_VALUE
                |   INTEGER_VALUE
                |   FLOAT_VALUE
                |   BOOL_VALUE
                |   NULL;