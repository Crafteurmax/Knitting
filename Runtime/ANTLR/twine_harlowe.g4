// base from https://github.com/devan-srinivasan/twine-harlowe-compiler
grammar twine_harlowe;

passage : stmt+ EOF;

stmt: conditional_block | (LPAR WS* command WS* RBRACK) | (LPAR WS* command WS* RPAR) | value | text;

conditional_block : ifblock WS* elseifblock* WS* elseblock? ;

ifblock : LPAR IF WS* startExpr WS* RPAR WS* LBRACK stmt* RBRACK;
elseifblock : LPAR ELSEIF WS* startExpr WS* RPAR WS* LBRACK stmt* RBRACK;
elseblock : LPAR ELSE WS* RPAR WS* LBRACK stmt* RBRACK;

command : set 
        | print 
        | lowercase
        | cond
        | execute
        | either
        | nth
        | unless
        | image
        | sound
        ;

set : SET WS* variable WS* TO WS* startExpr;
print : PRINT WS* startExpr;
lowercase : LOWERCASE WS* startExpr;
cond : COND WS* startExpr WS* COMMA WS* startExpr (WS* COMMA WS* startExpr WS* COMMA WS* startExpr)* WS* COMMA WS* startExpr;
execute : EXECUTE WS* startExpr;
either : EITHER WS* startExpr (WS* COMMA WS* startExpr)*;
nth : NTH WS* startExpr (WS* COMMA WS* startExpr)*;
unless : UNLESS WS* startExpr WS* RPAR LBRACK WS* stmt*;
image : IMAGE WS* startExpr ;
sound : SOUND WS* startExpr ;

startExpr : expr;

expr  : expr WS* ASTERIK WS* expr                           # Multiplication  
      | expr WS* FSLASH WS* expr                            # Division
      | expr WS* PLUS WS* expr                              # Addition
      | expr WS* MINUS WS* expr                             # Substraction 
      | expr WS* (AND | OR) WS* expr                        # BooleanLogic
      | expr WS* (IS | ISNOT | GT | LT | GE | LE) WS* expr  # Comparison
      | LPAR WS* expr WS* RPAR                              # Parentheses
      | value                                               # ExprValue
      ;

value: array | literal;

array :  ARR_START WS* expr ( WS* COMMA WS* expr )* WS* ; 
literal :  num | string | boolean | variable;

variable : DS ~(WS)+ ;

num : floating | integer ;

floating : MINUS? DIGIT* DOT DIGIT+ ;
integer : MINUS? DIGIT+ ;
string : (DOUBLE_QUOTE ~(DOUBLE_QUOTE)* DOUBLE_QUOTE) | (QUOTE ~(QUOTE)* QUOTE) ;
boolean : True | False;

text:   (ALL_ELSE 
        | ALPHA | DIGIT 
        | LPAR | RPAR | LBRACK | RBRACK 
        | DOT | COMMA | DOUBLE_QUOTE | TO 
        | MINUS | ASTERIK | PLUS | FSLASH 
        | AND | OR | IS | ISNOT
        | UNDERSCORE | DS 
        | WS)+;

// LEXER
LBRACK: '[';
RBRACK: ']';
LPAR: '(';
RPAR: ')';
DOUBLE_QUOTE: '"';
QUOTE: '\'';
UNDERSCORE: '_';
DS: '$';
DOT: '.';
COMMA: ',';
IF: 'if:';
ELSEIF: 'else-if:';
ELSE: 'else:';
SET: 'set:';
PRINT: 'print:';
LOWERCASE: 'lowercase:';
COND: 'cond:';
EXECUTE : 'execute:';
EITHER : 'either:';
NTH : 'nth:';
UNLESS : 'unless:';
IMAGE : 'image:';
SOUND : 'sound:';
TO: ' to ';
ARR_START: 'a:';
AND: 'and';
OR: 'or';
GT: '>';
LT: '<';
GE: '>=';
LE: '<=';
IS: 'is';
ISNOT: 'is not';
PLUS: '+';
MINUS: '-';
ASTERIK: '*';
FSLASH: '/';
ARROW: '->';
True: 'true';
False: 'false'; 

WS : [ \t\u000C\r\n];

ALPHA: [a-zA-Z];
DIGIT: [0-9];

ALL_ELSE: .;