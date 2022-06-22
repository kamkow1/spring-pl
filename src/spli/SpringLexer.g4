lexer grammar SpringLexer;

DEF                 : 'def';
DO                  : 'do';
END                 : 'end';
RETURN              : ('return' | 'exit with');
IF                  : 'if';
ELSE                : 'else';
ELIF                : 'elif';
AND                 : 'and';
OR                  : 'or';

STRING_VALUE        : '"' (~[\\"\r\n])* '"';
INTEGER_VALUE       : '-'? '0'..'9'+;
FLOAT_VALUE         : '-'? ('0'..'9')+ '.' ('0'..'9')*;
BOOL_VALUE          : ('True' | 'False');
NULL                : 'Null';

TERMINATOR          : ';';
DOLLAR              : '$';
ARROW               : '->';
LSQBR               : '[';
RSQBR               : ']';
LPAREN              : '(';
RPAREN              : ')';
COMMA               : ',';
DOT                 : '.';
EQUAL               : '==';
NOT_EQUAL           : '!=';
GREATER             : '>';
GREATER_EQUAL       : '>=';
LESS                : '<';
LESS_EQUAL          : '<=';
EXC_MARK            : '!';
PLUS                : '+';
MINUS               : '-';
MULT                : '*';
DIV                 : '/';
POW                 : '^';

IDENTIFIER          : ('a'..'z' | 'A'..'Z' | '_') ('a'..'z' | 'A' .. 'Z' | '0'..'9' | '_')*;
WHITESPACE          : [ \r\n\t]+    -> skip;
COMMENT             : '#*' .*? '*#' -> channel(HIDDEN);
LINE_COMMENT        : '#' ~[\r\n]*  -> channel(HIDDEN);