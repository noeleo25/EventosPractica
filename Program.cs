using System;
using System.Reflection.Metadata;

namespace PracticaEventos
{ 
    public enum TipoAlerta //add before
    {
        Error = 1,
        Advertencia = 2,
        Exito = 3
    }
    public enum TipoPago //add before
    {
        Tarjeta = 1,
        Transferencia = 2,
        Otros = 3
    }
    public delegate void CambioFormaPago(TipoPago tipoPago, TipoAlerta tipoAlerta); //declare the event handler signature
    //despues se llamara CambioFormaPagoHandler


    class Program //clase subscriptora
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ingresa una forma de pago: ");
            Console.WriteLine("1 - Tarjeta \n2 - Transferencia \n3 - Otros ");
            string tipoPago = Console.ReadLine();

            FormaDePago fm = new FormaDePago();
            fm.CambioFormaPago += fm_seleccionFormaPago;

            //la sig linea se agrega en el tema de chained events
            fm.CambioFormaPago += fm_continuarProcesoPago;

            fm.Tipo = (TipoPago)Enum.Parse(typeof(TipoPago), tipoPago);
            //fm.CambioFormaPago += fm_continuarProcesoPago; // para comprobar que el orden importa
        }
        
         static void fm_seleccionFormaPago(TipoPago tipo, TipoAlerta tipoAlerta) //eventhandler
        {
            if (tipoAlerta.Equals(TipoAlerta.Error))
                Console.WriteLine("Error - Elegiste una forma de pago incorrecta ");
            else if(tipoAlerta.Equals(TipoAlerta.Exito))
                Console.WriteLine("Nueva forma de pago seleccionada: {0}", tipo.ToString());
        }

        
        static void fm_continuarProcesoPago(TipoPago tipo, TipoAlerta tipoAlerta)
        {
            bool estatus = false;
            if (tipoAlerta.Equals(TipoAlerta.Exito))
            {
                Console.WriteLine("Continuando con el proceso de pago por ", tipo.ToString());
                Console.WriteLine("Presione x para continuar..", tipo.ToString());
                string tipoPago = Console.ReadLine();
                if(tipoPago == "x")
                    estatus = true; 
            }
            Console.WriteLine("Confirmacion recibida, estatus de la operacion {0}" , estatus ? "confirmada" : "cancelada");
        }
    }
    public class FormaDePago //event broadcaster/publisher/emisora
    {
        //banco
        //cuenta origen
        private TipoPago tipo; //atributo
        public event CambioFormaPago CambioFormaPago; //declara variable/propiedad como evento
        public TipoPago Tipo { //propiedad
            get
            {
                return tipo;
            }
            set
            {
                TipoAlerta tipoAlerta = TipoAlerta.Error;
                if ( value.Equals(TipoPago.Tarjeta)
                    || value.Equals(TipoPago.Transferencia)
                    || value.Equals(TipoPago.Otros)){
                    tipo = value;
                    tipoAlerta = TipoAlerta.Exito;
                }
                //CambioFormaPago(tipo, tipoAlerta);
                //2 para chain/unchain events
                CambioFormaPago(tipo, tipoAlerta);
            } 
        }
    }
}
