using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OLC1_SQL.Program;

namespace OLC1_SQL
{
    class Scanner
    {
        private char[] entrada;
        private List<Token> tokens;
        private List<Token> errores;
        private int estado, fila, columna, tempFila, tempColumna;
        private String lexema;

        private int CRLF = 655370;


        public Scanner(String entrada)
        {
            this.entrada = entrada.ToCharArray();
          
            this.tokens = new List<Token>();
            this.errores = new List<Token>();
        }

        public List<Token> Scan()
        {
            this.fila = 1;
            this.columna = 1;
            this.estado = 0;
            this.tempFila = 0;
            this.lexema = "";

            char c = ' ';

            for(int i = 0; i < entrada.Length; i++)
            {
                c = entrada[i];

                switch (estado)
                {
                    case 0: //Estado inicial
                        this.tempFila = fila;
                        this.tempColumna = columna;
                        lexema = "";

                        if (c.Equals('/'))
                        {
                            estado = 10;
                            lexema += c;
                        }
                        else if (c.Equals('-'))
                        {
                            estado = 20;
                            lexema += c;
                        }
                        else if (Char.IsLetter(c))
                        {
                            estado = 30;
                            lexema += c;
                        }
                        else if (c.Equals('='))
                        {
                            estado = 40;
                        }
                        else if (c.Equals('<'))
                        {
                            estado = 110;
                        }
                        else if (c.Equals('>'))
                        {
                            estado = 120;
                        }
                        else if (c.Equals('!'))
                        {
                            estado = 130;
                        }
                        else if (c.Equals('('))
                        {
                            agregarToken(TokenSQL.CL_PARENTESIS_1, "(", fila, columna);
                        }
                        else if (c.Equals(')'))
                        {
                            agregarToken(TokenSQL.CL_PARENTESIS_2, ")", fila, columna);
                        }
                        else if (c.Equals(','))
                        {
                            agregarToken(TokenSQL.CL_COMA, ",", fila, columna);
                        }
                        else if (c.Equals(';'))
                        {
                            agregarToken(TokenSQL.CL_FL, ";", fila, columna);
                        }
                        else if (c.Equals('.'))
                        {
                            agregarToken(TokenSQL.CL_PUNTO, ".", fila, columna);
                        }
                        else if (c.Equals('*'))
                        {
                            agregarToken(TokenSQL.CL_POR, "*", fila, columna);
                        }
                        else if (Char.IsDigit(c))
                        {
                            estado = 80;
                            lexema += c;
                        }
                        else if (c == 34) // "
                        {
                            estado = 70;
                            lexema += c;
                        }
                        else if(c == 39) // '
                        {
                            estado = 50;
                            lexema += c;
                        }
                        else if (c.Equals(10) || c.Equals(13) || c.GetHashCode().Equals(CRLF)) //CRLF
                        {
                            Console.WriteLine("CRLF");
                            nuevaLinea();
                        }
                        else if (Char.IsWhiteSpace(c)) //WS
                        {
                            //No hacer nada
                            Console.WriteLine("Espacio");
                        }
                        else if(c == '\t') //TAB
                        {
                            //No hacer nada
                            Console.WriteLine("TAB");
                        }
                        else
                        {
                            agregarError(c.ToString(), fila, columna);
                        }

                        break;
                    case 10: //Estado 10, Comentario bloque
                        if (c.Equals('*'))
                        {
                            estado = 11;
                            lexema += c;
                        }
                        else
                        {
                            //Error lexico /
                            agregarError(lexema, tempFila, tempColumna);
                            estado0();
                            i--;
                        }
                        break;
                    case 11: //Estado 11, Comentario bloque
                        if (c.Equals('*'))
                        {
                            estado = 12;
                        }

                        if (c.Equals(10) || c.Equals(13) || c.GetHashCode().Equals(CRLF))
                        {
                            nuevaLinea();
                        }

                        lexema += c;
                        break;
                    case 12: //Estado 12, Comentario bloque
                        if (c.Equals('/'))
                        {
                            estado = 13;
                        }
                        else
                        {
                            estado = 11;
                        }

                        lexema += c;
                        break;
                    case 13: //Estado 13, Aceptar comentario bloque
                        agregarToken(TokenSQL.COMENTARIO_BLOQUE, lexema, tempFila, tempColumna);
                        estado0();
                        i--;
                        break;

                    case 20: //Estado 20, comentario linea
                        if (c.Equals('-'))
                        {
                            estado = 21;
                            lexema += c;
                        }
                        else
                        {
                            //Error léxico -
                            agregarError(lexema, tempFila, tempColumna);
                            estado0();
                            i--;
                        }
                        break;
                    case 21: //Estado 21, comentario linea y aceptación de comentario
                        if (c.Equals(10) || c.Equals(13) || c.GetHashCode().Equals(CRLF))
                        {
                            agregarToken(TokenSQL.COMENTARIO_LINEA, lexema, tempFila, tempColumna);
                            estado0();
                            i--;
                        }
                        else
                        {
                            lexema += c;
                        }
                        break;

                    case 30: //Estado 30, variable o palabra reservada y aceptación
                        if (Char.IsLetterOrDigit(c) || c.Equals('_'))
                        {
                            lexema += c;
                        }
                        else
                        {
                            agregarToken(TokenSQL.ID, lexema, tempFila, tempColumna);
                            estado0();
                            i--;
                        }
                        break;

                    case 40: //Estado 40, aceptar =
                        agregarToken(TokenSQL.CL_IGUAL, "=", tempFila, tempColumna);
                        estado0();
                        i--;
                        break;

                    case 110: //Estado 110, aceptar < 
                        if (c.Equals('='))
                        {
                            estado = 111;
                        }
                        else
                        {
                            agregarToken(TokenSQL.CL_MENOR, "<", tempFila, tempColumna);
                            estado0();
                            i--;
                        }
                        break;
                    case 111: //Estado 111, acepatr <=
                        agregarToken(TokenSQL.CL_MENOR_IGUAL, "<=", tempFila, tempColumna);
                        estado0();
                        i--;
                        break;

                    case 120: //Estado 120, aceptar >
                        if (c.Equals('='))
                        {
                            estado = 121;
                        } 
                        else
                        {
                            agregarToken(TokenSQL.CL_MAYOR, ">", tempFila, tempColumna);
                            estado0();
                            i--;
                        }
                        break;
                    case 121: //Estado 121, aceptar >=
                        agregarToken(TokenSQL.CL_MAYOR_IGUAL, ">=", tempFila, tempColumna);
                        estado = 0;
                        fila--;
                        i--;
                        break;

                    case 130: //Estado 130, diferente
                        if (c.Equals('='))
                        {
                            estado = 131;
                        }
                        else
                        {
                            //Error !
                            agregarError("!", tempFila, tempColumna);
                            estado0();
                            i--;
                        }
                        break;
                    case 131: //Estado 131, aceptar !=
                        agregarToken(TokenSQL.CL_DIFERENTE, "!=", tempFila, tempColumna);
                        estado0();
                        i--;
                        break;

                    case 80: //Estado 80, aceptar entero
                        if (c.Equals('.'))
                        {
                            estado = 81;
                            lexema += c;
                        }
                        else if (Char.IsDigit(c))
                        {
                            lexema += c;
                        } 
                        else
                        {
                            agregarToken(TokenSQL.ENTERO, lexema, tempFila, tempColumna);
                            estado0();
                            i--;
                        }
                        break;
                    case 81: //Estado 81, aceptar flotante
                        if (Char.IsDigit(c))
                        {
                            lexema += c;
                        }
                        else
                        {
                            agregarToken(TokenSQL.FLOTANTE, lexema, tempFila, tempColumna);
                            estado0();
                            i--;
                        }
                        break;

                    case 70: //Estado 70, cadena
                        if(c == 34)
                        {
                            estado = 71;
                        }
                        lexema += c;
                        break;
                    case 71: //Estado 71, aceptar cadena
                        agregarToken(TokenSQL.CADENA, lexema, tempFila, tempColumna);
                        estado0();
                        i--;
                        break;

                    case 50: //Estado 50, fecha
                        if (Char.IsDigit(c))
                        {
                            estado = 51;
                            lexema += c;
                        }
                        else
                        {
                            agregarError(lexema, tempFila, tempColumna);
                            estado0();
                            i--;
                        }
                        break;
                    case 51: //Estado 51, fecha
                        if (char.IsDigit(c) || c.Equals('/'))
                        {
                            lexema += c;
                        } 
                        else if(c == 39)
                        {
                            estado = 52;
                            lexema += c;
                        }
                        else
                        {
                            agregarError(c.ToString(), fila, tempColumna);
                        }
                        break;
                    case 52: //Estado 52, aceptar fecha
                        agregarToken(TokenSQL.FECHA,lexema, tempFila, tempColumna);
                        estado0();
                        i--;
                        break;
                }

                columna++;
            }


            return this.tokens;
        }

        //Otros metodos
        private void estado0()
        {
            estado = 0;
            columna--;
        }

        private void nuevaLinea()
        {
            Console.WriteLine("Linea: " + fila);
            fila++;
            columna = 0;
        }
        private void agregarError(String lexema, int fila, int columna)
        {
            errores.Add(new Token(TokenSQL.ERROR_LEXICO, "El caracter: << " + lexema + " >> no pertenece al lenguaje", fila, columna));
        }

        private void agregarToken(TokenSQL token, String lexema, int fila, int columna)
        {
            if(token == TokenSQL.ID)
            {
                //Verificar si es una palabra reservada
                tokens.Add(new Token(validarToken(lexema), lexema, fila, columna));
            }
            else
            {
                tokens.Add(new Token(token, lexema, fila, columna));
            }
            Console.WriteLine("TempFila: " + tempFila + ", TempColumna: " + tempColumna);
        }

        private TokenSQL validarToken(String lexema)
        {
            switch (lexema.ToUpper())
            {
                case "ACTUALIZAR":
                    return TokenSQL.PR_ACTUALIZAR;
                case "CADENA":
                    return TokenSQL.PR_CADENA;
                case "COMO":
                    return TokenSQL.PR_COMO;
                case "CREAR":
                    return TokenSQL.PR_CREAR;
                case "DE":
                    return TokenSQL.PR_DE;
                case "DONDE":
                    return TokenSQL.PR_DONDE;
                case "EN":
                    return TokenSQL.PR_EN;
                case "ENTERO":
                    return TokenSQL.PR_ENTERO;
                case "ESTABLECER":
                    return TokenSQL.PR_ESTABLECER;
                case "FECHA":
                    return TokenSQL.PR_FECHA;
                case "FLOTANTE":
                    return TokenSQL.PR_FLOTANTE;
                case "INSERTAR":
                    return TokenSQL.PR_INSERTAR;
                case "O":
                    return TokenSQL.PR_O;
                case "SELECCIONAR":
                    return TokenSQL.PR_SELECCIONAR;
                case "TABLA":
                    return TokenSQL.PR_TABLA;
                case "VALORES":
                    return TokenSQL.PR_VALORES;
                case "Y":
                    return TokenSQL.PR_Y;
                default:
                    return TokenSQL.ID;
            }
        }

        public List<Token> getErrores()
        {
            return this.errores;
        }
    }
}
