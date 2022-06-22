parser grammar ModuleParser;

options {
    tokenVocab = './ModuleLexer/ModuleLexer';
}

parse: content;

content: include?;

include: (OPEN_INCLUDES include_path* END_INCLUDES);

include_path: INCLUDE STRING_VALUE TERMINATOR;