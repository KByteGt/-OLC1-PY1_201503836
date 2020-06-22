using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static OLC1_SQL.Program;

namespace OLC1_SQL
{
    class Parser
    {
        //private NodoAST raiz;
        private List<Token> listaTokens;
        private List<Token> listaErrores;
        private NodoAST raiz;
        public Parser(List<Token> lista)
        {
            this.listaTokens = lista;
            this.raiz = new NodoAST(0,"RAIZ");
            this.listaErrores = new List<Token>();
        }
        /// ////////////////////////////////////////////////////////////////////////
        Token preanalisis, temp;
        int index = 1, id = 1;
        String errorMessage = "";

        public NodoAST Pars()
        {
            preanalisis = listaTokens[0];
            //Ir a estado INICIO
            INICIO(this.raiz);

            return raiz;
        }

        private void match(TokenSQL terminal, NodoAST raiz)
        {
            /* si preanalisis = terminal
             * entonces preanalisis = siguiente toekn
             * sino error sintactico
             */
            Console.WriteLine("Match: " + terminal + " == " + preanalisis.getToken());
            if (preanalisis.getToken() == terminal)
            {
                raiz.insertarHijo(new NodoAST(preanalisis)); // Insertamos token en el árbol AST
                preanalisis = getToken();
                index++;
            }
            else
            {
                //Error
                insertarError("un token[" + terminal.ToString() + "]", preanalisis);
                panico();
            }
        }

        private Token getToken()
        {
            if(index < listaTokens.Count)
            {
                return listaTokens[index];
            }

            return new Token(TokenSQL.EOF,"",0,0);
        }

        /// 
        private void INICIO(NodoAST raiz)
        {
            /* INICIO 	⇒	CARGAR_TABLAS
             *          |	AREA_CONSULTAS
             * primeros de (INICIO) ⇒ primeros de (CARGAR_TABLAS) U primeros de (AREA_CONSULTAS) 
             *          ⇒ {CREAR, INSERTAR, SELECCIONAR, ELIMINAR, ACTUALIZAR}
             */
            switch (preanalisis.getToken())
            {
                case TokenSQL.PR_CREAR:
                    CARGAR_TABLAS(raiz);
                    break;
                case TokenSQL.PR_INSERTAR:
                    CARGAR_TABLAS(raiz);
                    break;
                case TokenSQL.PR_SELECCIONAR:
                    AREA_CONSULTAS(raiz);
                    break;
                case TokenSQL.PR_ELIMINAR:
                    AREA_CONSULTAS(raiz);
                    break;
                case TokenSQL.PR_ACTUALIZAR:
                    AREA_CONSULTAS(raiz);
                    break;
                default:
                    //Error 1
                    insertarError("una instrucción a ejecutar", preanalisis);
                    panico();
                    break;
            }
        }

        private void CARGAR_TABLAS(NodoAST raiz)
        {
            /*CARGAR_TABLAS 	⇒	CREAR_TABLA  CARGA_TABLAS
			 *                  |	I_INSERTAR  CARGAR_TABLAS
			 *                  |
             */
            switch (preanalisis.getToken())
            {
                case TokenSQL.PR_CREAR: // Palabra reservada CREAR -> primeros de CREAR_TABLA
                    CREAR_TABLA(raiz);
                    CARGAR_TABLAS(raiz);
                    break;
                case TokenSQL.PR_INSERTAR: //Palabra reservada INSERTAR -> primeros de I_INSERTAR
                    I_INSERTAR(raiz);
                    CARGAR_TABLAS(raiz);
                    break;
                default:
                    //No hacer nada
                    break;
            }
        }

        private void CREAR_TABLA(NodoAST raiz)
        {
            /*CREAR TABLA ID ( L_CAMPOS ) ; */
            NodoAST nodo = new NodoAST(id, "INSTRUCCION CREAR TABLA");
            sumarID();

            match(TokenSQL.PR_CREAR, nodo);
            match(TokenSQL.PR_TABLA, nodo);
            match(TokenSQL.ID, nodo);
            match(TokenSQL.CL_PARENTESIS_1, nodo);

            NodoAST nodoB = new NodoAST(id, "LISTA DE CAMPOS");
            sumarID();
            L_CAMPOS(nodoB);
            nodo.insertarHijo(nodoB);

            match(TokenSQL.CL_PARENTESIS_2, nodo);
            match(TokenSQL.CL_FL, nodo);

            raiz.insertarHijo(nodo);
        }

        private void L_CAMPOS(NodoAST raiz)
        {
            /*L_CAMPOS		⇒	ID TIPO_DATO L_CAMPOS_1*/
            match(TokenSQL.ID, raiz);

            TIPO_DATO(raiz);

            L_CAMPOS_1(raiz);
        }

        private void L_CAMPOS_1(NodoAST raiz)
        {
            /*L_CAMPOS_1		⇒	, L_CAMPOS
			 *                  |
			 */
            switch (preanalisis.getToken())
            {
                case TokenSQL.CL_COMA: // Coma (,)
                    match(TokenSQL.CL_COMA, raiz);
                    L_CAMPOS(raiz);
                    break;
                default:
                    //No hacer nada
                    break;
            }
        }

        private void I_INSERTAR(NodoAST raiz)
        {
            /*I_INSERTAR 		⇒	INSERTAR EN ID VALORES ( L_VALORES ) ;*/
            NodoAST nodo = new NodoAST(id, "INSTRUCCION INSERTAR");
            sumarID();

            match(TokenSQL.PR_INSERTAR, nodo);
            match(TokenSQL.PR_EN, nodo);
            match(TokenSQL.ID, nodo);
            match(TokenSQL.PR_VALORES, nodo);
            match(TokenSQL.CL_PARENTESIS_1, nodo);

            NodoAST nodoA = new NodoAST(id, "LISTA DE VALORES");
            sumarID();
            L_VALORES(nodoA);
            nodo.insertarHijo(nodoA);

            match(TokenSQL.CL_PARENTESIS_2, nodo);
            match(TokenSQL.CL_FL, nodo);

            raiz.insertarHijo(nodo);
        }

        private void L_VALORES(NodoAST raiz)
        {
            /*L_VALORES 		⇒	VALOR L_VALORES_1*/
            switch (preanalisis.getToken())
            {
                case TokenSQL.ENTERO: // Tipo de dato Entero -> primeros de VALOR
                    VALOR(raiz);
                    L_VALORES_1(raiz);
                    break;
                case TokenSQL.CADENA: // Tipo de dato Cadena -> primeros de VALOR
                    VALOR(raiz);
                    L_VALORES_1(raiz);
                    break;
                case TokenSQL.FLOTANTE: // Tipo de dato Flotante -> primeros de VALOR
                    VALOR(raiz);
                    L_VALORES_1(raiz);
                    break;
                case TokenSQL.FECHA: // Tipo de dato Fecha -> primeros de VALOR
                    VALOR(raiz);
                    L_VALORES_1(raiz);
                    break;
                default:
                    insertarError("un valor de dato", preanalisis);
                    panico();
                    break;
            }
        }

        private void L_VALORES_1(NodoAST raiz)
        {
            /*L_VALORES_1		⇒	, L_VALORES
			 *                  |
			 */
            switch (preanalisis.getToken())
            {
                case TokenSQL.CL_COMA: // Coma (,)
                    match(TokenSQL.CL_COMA, raiz);
                    L_VALORES(raiz);
                    break;
                default:
                    //No hacer nada
                    break;
            }
        }

        /// 

        private void AREA_CONSULTAS(NodoAST raiz)
        {
            /*AREA_CONSULTAS	⇒	I_SELECCIONAR AREA_CONSULTA
			 *                  |	I_ELIMINAR AREA_CONSULTA
			 *                  |	I_ACTUALIZAR AREA_CONSULTA
			 *                  |
             */
            switch (preanalisis.getToken())
            {
                case TokenSQL.PR_SELECCIONAR: // Palabra reservada SELECCIONAR -> Primeros de I_SELECCIONAR
                    I_SELECCIONAR(raiz);
                    AREA_CONSULTAS(raiz);
                    break;
                case TokenSQL.PR_ELIMINAR: // Palabra reservada ELIMINAR -> primeros de I_ELIMINAR
                    I_ELIMINAR(raiz);
                    AREA_CONSULTAS(raiz);
                    break;
                case TokenSQL.PR_ACTUALIZAR: // Palabra reservada ACTUALIZAR -> primeros de I_ACTUALIZAR
                    I_ACTUALIZAR(raiz);
                    AREA_CONSULTAS(raiz);
                    break;
                default:
                    //No hacer nada
                    break;
            }
        }

        private void I_SELECCIONAR(NodoAST raiz)
        {
            /*I_SELECCIONAR	⇒	SELECCIONAR OP_SELECT DE L_TABLAS I_SELECCIONAR_1 ;*/
            NodoAST nodo = new NodoAST(id, "INSTRUCCION SELECCIONAR");
            sumarID();

            match(TokenSQL.PR_SELECCIONAR, nodo);

            NodoAST nodoA = new NodoAST(id, "COLUMNAS");
            sumarID();
            OP_SELECT(nodoA);
            nodo.insertarHijo(nodoA);

            match(TokenSQL.PR_DE, nodo);

            NodoAST nodoB = new NodoAST(id, "TABLAS");
            sumarID();
            L_TABLAS(nodoB);
            nodo.insertarHijo(nodoB);

            NodoAST nodoC = new NodoAST(id, "DONDE");
            sumarID();
            I_SELECCIONAR_1(nodoC);
            nodo.insertarHijo(nodoC);

            match(TokenSQL.CL_FL, nodo);

            raiz.insertarHijo(nodo);
        }

        private void I_SELECCIONAR_1(NodoAST raiz)
        {
            /*I_SELECCIONAR_1	⇒	DONDE CONDICION I_SELECCIONAR_2
			 *                  |	
			 */
            switch (preanalisis.getToken())
            {
                case TokenSQL.PR_DONDE: // Palabra reservada DONDE
                    match(TokenSQL.PR_DONDE, raiz);

                    CONDICION(raiz);

                    I_SELECCIONAR_2(raiz);
                    break;
                default:
                    //No hace nada
                    break;
            }
        }

        private void I_SELECCIONAR_2(NodoAST raiz)
        {
            /*I_SELECCIONAR_2	⇒ 	, L_YO
			 *                  |
			 */
            switch (preanalisis.getToken())
            {
                case TokenSQL.CL_COMA: // Coma (,)
                    match(TokenSQL.CL_COMA, raiz);
                    L_YO(raiz);
                    break;
                default:
                    //No hacer nada
                    break;
            }
        }

        private void OP_SELECT(NodoAST raiz)
        {
            /*OP_SELECT		⇒	L_COLUMNAS
			 *              |	*
			 * 
			 * primeros de (OP_SELECT) ⇒ primeros de (L_COLUMNAS) ⇒ {ID, *}
			 */
            switch (preanalisis.getToken())
            {
                case TokenSQL.CL_POR: // Por (*)
                    match(TokenSQL.CL_POR, raiz);
                    break;
                case TokenSQL.ID: //ID -> primeros de L_COLUMNAS
                    L_COLUMNAS(raiz);
                    break;
                default: //Error
                    insertarError(" un ID o *", preanalisis);
                    panico();
                    break;
            }
        }

        private void L_COLUMNAS(NodoAST raiz)
        {
            /*L_COLUMNAS	⇒	ID L_COLUMNAS_1*/
            match(TokenSQL.ID, raiz);
            L_COLUMNAS_1(raiz);
        }

        private void L_COLUMNAS_1(NodoAST raiz)
        {
            /*L_COLUMNAS_1	⇒	I_COMO L_COLUMNAS_2
			 *              |	. ID I_COMO L_COLUMNAS_2
			 *
			 * primeros de (L_COLUMNAS_1) ⇒ primeros de (I_COMO) U primeros de (L_COLUMNAS_2) ⇒ {COMO, ,, . }
			 */
            switch (preanalisis.getToken())
            {
                case TokenSQL.CL_PUNTO: // punto (.)
                    match(TokenSQL.CL_PUNTO, raiz);

                    I_COMO(raiz);

                    L_COLUMNAS_2(raiz);
                    break;
                case TokenSQL.PR_COMO: //Palabra reservada COMO -> primeros de I_COMO
                    I_COMO(raiz);
                    L_COLUMNAS_2(raiz);
                    break;
                case TokenSQL.CL_COMA: // Coma (,) -> primeros de L_COLUMNA_2
                    //I_COMO(raiz);
                    L_COLUMNAS_2(raiz);
                    break;
                default:
                    //Error
                    insertarError(" (COMO|,|.)", preanalisis);
                    panico();
                    break;
            }
        }

        private void L_COLUMNAS_2(NodoAST raiz)
        {
            /*L_COLUMNAS_2 	⇒	, L_COLUMNAS
             *              |
             */
            switch (preanalisis.getToken())
            {
                case TokenSQL.CL_COMA: // Coma (,)
                    match(TokenSQL.CL_COMA, raiz);
                    L_COLUMNAS(raiz);
                    break;
                default:
                    //No hacer nada
                    break;
            }
        }

        private void I_COMO(NodoAST raiz)
        {
            /*I_COMO		⇒ 	COMO ID
			 *              |
			 */
            switch (preanalisis.getToken())
            {
                case TokenSQL.PR_COMO: // Palabra reservada COMO
                    match(TokenSQL.PR_COMO, raiz);
                    match(TokenSQL.ID, raiz);
                    break;
                default:
                    //No hacer nada
                    break;
            }
        }

        private void L_TABLAS(NodoAST raiz)
        {
            /*L_TABLAS		⇒	ID L_TABLAS_1*/
            match(TokenSQL.ID, raiz);

            L_TABLAS_1(raiz);
        }

        private void L_TABLAS_1(NodoAST raiz)
        {
            /*L_TABLAS_1		⇒	, L_TABLAS
			 *                  |
			 */
            switch (preanalisis.getToken())
            {
                case TokenSQL.CL_COMA: // Coma (,)
                    match(TokenSQL.CL_COMA, raiz);

                    L_TABLAS(raiz);
                    break;
                default:
                    //No hacer nada
                    break;
            }
        }

        private void I_ELIMINAR(NodoAST raiz)
        {
            /*I_ELIMINAR		⇒	ELIMINAR DE ID I_ELIMINAR_1 ;*/
            NodoAST nodo = new NodoAST(id, "INSTRUCCION ELIMINAR");
            sumarID();

            match(TokenSQL.PR_ELIMINAR, nodo);
            match(TokenSQL.PR_DE, nodo);
            match(TokenSQL.ID, nodo);

            NodoAST nodoA = new NodoAST(id, "DONDE");
            sumarID();
            I_ELIMINAR_1(nodoA);
            nodo.insertarHijo(nodoA);

            match(TokenSQL.CL_FL, nodo);

            raiz.insertarHijo(nodo);
        }

        private void I_ELIMINAR_1(NodoAST raiz)
        {
            /*I_ELIMINAR_1	⇒	DONDE CONDICION I_ELIMINAR_2
			 *              |             
			 */
            switch (preanalisis.getToken())
            {
                case TokenSQL.PR_DONDE: // Palabra reservada DONDE
                    match(TokenSQL.PR_DONDE, raiz);

                    CONDICION(raiz);

                    I_ELIMINAR_2(raiz);
                    break;
                default:
                    //No hacer nada
                    break;
            }
        }

        private void I_ELIMINAR_2(NodoAST raiz)
        {
            /*I_ELIMINAR_2	⇒	, L_YO
			 *              |             
			 */
            switch (preanalisis.getToken())
            {
                case TokenSQL.CL_COMA: // Coma (,)
                    match(TokenSQL.CL_COMA, raiz);

                    L_YO(raiz);
                    break;
                default:
                    //No hacer nada
                    break;
            }
        }
        
        private void I_ACTUALIZAR(NodoAST raiz)
        {
            /*I_ACTUALIZAR	⇒	ACTUALIZAR ID ESTABLECER ( L_ESTABLECER ) I_ACTUALIZAR_1 ;*/
            NodoAST nodo = new NodoAST(id,"INSTRUCCION ACTUALIZAR");
            sumarID();

            match(TokenSQL.PR_ACTUALIZAR, nodo);
            match(TokenSQL.ID, nodo);
            match(TokenSQL.PR_ESTABLECER, nodo);
            match(TokenSQL.CL_PARENTESIS_1, nodo);

            NodoAST nodoA = new NodoAST(id, "LISTA ESTABLECER");
            sumarID();
            L_ESTABLECER(nodoA);
            nodo.insertarHijo(nodoA);

            match(TokenSQL.CL_PARENTESIS_2, nodo);

            NodoAST nodoB = new NodoAST(id, "DONDE");
            sumarID();
            I_ACTUALIZAR(nodoB);
            nodo.insertarHijo(nodoB);

            match(TokenSQL.CL_FL, nodo);

            raiz.insertarHijo(nodo);
        }

        private void I_ACTUALIZAR_1(NodoAST raiz)
        {
            /*I_ACTUALIZAR_1	⇒	DONDE CONDICION I_ACTUALIZAR_2
			 *                  |
             */
            switch (preanalisis.getToken())
            {
                case TokenSQL.PR_DONDE: // Palabra reservada DONDE
                    match(TokenSQL.PR_DONDE, raiz);

                    CONDICION(raiz);

                    I_ACTUALIZAR_2(raiz);
                    break;
                default:
                    //No hacer nada
                    break;
            }
        }

        private void I_ACTUALIZAR_2(NodoAST raiz)
        {
            /*I_ACTUALIZAR_2	⇒	, L_YO
			 *                  |
			 */
            switch (preanalisis.getToken())
            {
                case TokenSQL.CL_COMA: // Coma (,)
                    match(TokenSQL.CL_COMA, raiz);

                    L_YO(raiz);
                    break;
                default:
                    //No hacer nada
                    break;
            }
        }

        private void L_YO(NodoAST raiz)
        {
            /*L_YO			⇒	Y CONDICION
			 *              |	O CONDICION
			 */
            switch (preanalisis.getToken())
            {
                case TokenSQL.PR_Y: // Palabra reservada Y
                    match(TokenSQL.PR_Y, raiz);

                    CONDICION(raiz);
                    break;
                case TokenSQL.PR_O: // Palabra reservada O
                    match(TokenSQL.PR_O, raiz);

                    CONDICION(raiz);
                    break;
                default:
                    //Error
                    insertarError("(Y | O)", preanalisis);
                    panico();
                    break;
            }
        }

        private void L_ESTABLECER(NodoAST raiz)
        {
            /*L_ESTABLECER 	⇒	ID = VALOR L_ESTABLECER_1*/

            match(TokenSQL.ID, raiz);
            match(TokenSQL.CL_IGUAL, raiz);

            VALOR(raiz);

            L_ESTABLECER_1(raiz);
        }

        private void L_ESTABLECER_1(NodoAST raiz)
        {
            /*L_ESTABLECER_1	⇒	, L_ESTABLECER
			 *                  |                 
			 */
            switch (preanalisis.getToken())
            {
                case TokenSQL.CL_COMA: // Coma (,)
                    match(TokenSQL.CL_COMA, raiz);

                    L_ESTABLECER(raiz);

                    break;
                default:
                    //No hacer nada
                    break;
            }
        }

        private void TIPO_DATO(NodoAST raiz)
        {
            /*TIPO_DATO		⇒	ENTERO
			 *              |	CADENA
			 *              |	FLOTANTE
			 *              |	FECHA
			 */
            switch (preanalisis.getToken())
            {
                case TokenSQL.PR_ENTERO: // Palabra reservada Entero
                    match(TokenSQL.PR_ENTERO, raiz);
                    break;
                case TokenSQL.PR_CADENA: // Palabra reservada Cadena
                    match(TokenSQL.PR_CADENA, raiz);
                    break;
                case TokenSQL.PR_FLOTANTE: // Palabra reservada Flotante
                    match(TokenSQL.PR_FLOTANTE, raiz);
                    break;
                case TokenSQL.PR_FECHA: // Palabra reservada Fecha
                    match(TokenSQL.PR_FECHA, raiz);
                    break;
                default:
                    insertarError("un tipo de dato", preanalisis);
                    panico();
                    break;
            }
        }

        private void VALOR(NodoAST raiz)
        {
            /*VALOR 		⇒	ENTERO
			 *              |	CADENA
			 *              |	FLOTANTE
			 *              |	FECHA
			 */
            switch (preanalisis.getToken())
            {
                case TokenSQL.ENTERO: // Tipo de dato Entero
                    match(TokenSQL.ENTERO, raiz);
                    break;
                case TokenSQL.CADENA: // Tipo de dato Cadena
                    match(TokenSQL.CADENA, raiz);
                    break;
                case TokenSQL.FLOTANTE: // Tipo de dato Flotante
                    match(TokenSQL.FLOTANTE, raiz);
                    break;
                case TokenSQL.FECHA: // Tipo de dato Fecha
                    match(TokenSQL.FECHA, raiz);
                    break;
                default:
                    insertarError("un valor de dato", preanalisis);
                    panico();
                    break;
            }
        }

        private void CONDICION(NodoAST raiz)
        {
            /*CONDICION		⇒	ID OPERADOR VALOR*/
            NodoAST nodo = new NodoAST(id, "CONDICION");
            sumarID();

            match(TokenSQL.ID, nodo);

            OPERADOR(nodo);

            VALOR(nodo);

            raiz.insertarHijo(nodo);

        }

        private void OPERADOR(NodoAST raiz)
        {
            /*OPERADOR		⇒	< 
			 *              |	<= 
			 *              |	>
			 *              |	>= 
			 *              |	= 
			 *              |	!= 
			 */
            switch (preanalisis.getToken())
            {
                case TokenSQL.CL_MENOR: //<
                    match(TokenSQL.CL_MENOR, raiz);
                    break;
                case TokenSQL.CL_MENOR_IGUAL: //<=
                    match(TokenSQL.CL_MENOR_IGUAL, raiz);
                    break;
                case TokenSQL.CL_MAYOR: //>
                    match(TokenSQL.CL_MAYOR, raiz);
                    break;
                case TokenSQL.CL_MAYOR_IGUAL: //>=
                    match(TokenSQL.CL_MAYOR_IGUAL, raiz);
                    break;
                case TokenSQL.CL_IGUAL: // =
                    match(TokenSQL.CL_IGUAL, raiz);
                    break;
                case TokenSQL.CL_DIFERENTE: // !=
                    match(TokenSQL.CL_DIFERENTE, raiz);
                    break;
                default:
                    insertarError("un operador aritmético", preanalisis);
                    panico();
                    break;
            }
        }

        private void panico()
        {
            while(preanalisis.getToken() != TokenSQL.CL_FL && preanalisis != null)
            {
                //Saltar tokens hasta encontrar ";"
                preanalisis = getToken();
            }

            if(preanalisis.getToken() == TokenSQL.CL_FL)
            {
                //Ejecutar 
                Console.WriteLine(" **Token de SINC encontrado (;)");
            }
        }

        /// ////////////////////////////////////////////////////////////////////////

        private void sumarID()
        {
            id++;
        }

        private void insertarError(String valorCorrecto, Token token)
        {
            errorMessage = "Se esperaba " + valorCorrecto + ", se encontro << " + token.getLexema() + " >>";   
            listaErrores.Add(new Token(TokenSQL.ERROR_SINTACTICO, errorMessage, token.getFila(), token.getColumna()));
        }

        public List<Token> getErroes()
        {
            return listaErrores;
        }
    }
}
