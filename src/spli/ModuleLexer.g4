lexer grammar ModuleLexer;

OPEN_INCLUDES       : '[]';
INCLUDE             : 'include';
END_INCLUDES        : '[/]';

TERMINATOR          : ';';
STRING_VALUE        : '"' (~[\\"\r\n])* '"';

IDENTIFIER          : ('a'..'z' | 'A'..'Z' | '_') ('a'..'z' | 'A' .. 'Z' | '0'..'9' | '_')*;
WHITESPACE          : [ \r\n\t]+    -> skip;
COMMENT             : '/*' .*? '*/' -> channel(HIDDEN);
LINE_COMMENT        : '--' ~[\r\n]*  -> channel(HIDDEN);