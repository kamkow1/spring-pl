action=$1

jar_loc=""

if [[ -z "$2" ]]; then
  jar_loc="/usr/local/bin/antlr-4.10.1-complete.jar"
else
  jar_loc=$2
fi

if [ "$action" = "lexer" ]; then
  eval "java -jar $jar_loc ./SpringLexer.g4 -Dlanguage=CSharp -o ./Lexer"
  eval "java -jar $jar_loc ./ModuleLexer.g4 -Dlanguage=CSharp -o ./ModuleLexer"
elif [ "$action" = "parser" ]; then
  eval "java -jar $jar_loc ./SpringParser.g4 -Dlanguage=CSharp -o ./Parser -no-listener -visitor"
  eval "java -jar $jar_loc ./ModuleParser.g4 -Dlanguage=CSharp -o ./ModuleParser -no-listener -visitor"
else
  echo "action not supported"
fi