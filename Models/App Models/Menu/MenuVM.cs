/************************************************************************************************************
 * Descripción: Clase Contrato catAgendaVM.
 * Observaciones: 
 * Creado por: mc.
 * Historial de Revisiones:
 * -----------------------------------------------------------------------------------------------
 * Fecha        Evento / Método Autor   Descripción
 * -----------------------------------------------------------------------------------------------
 * 05/12/2016					Implementación inicial.
 * -----------------------------------------------------------------------------------------------
*/
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Proyecto.Models
{
    public class MenuVM 
    {

        #region Atributos
        int AID;
        int? APadreID;
        string ANombre;
        int? AOrden;
        string AIcono;
        string AAccion;
        string AControlador;
        DateTime AFechaAlta;
        int? AmnuId;
        bool AActivo;

        #endregion Atributos

        #region Propiedades - Get/Set
        

        public int ID
        {
            get { return AID; }
            set { AID = value; }
        }

        public int? PadreID
        {
            get { return APadreID; }
            set { APadreID = value; }
        }
                

        public string Nombre
        {
            get { return ANombre; }
            set { ANombre = value; }
        }


        public int? Orden
        {
            get { return AOrden; }
            set { AOrden = value; }
        }


        public string Icono
        {
            get { return AIcono; }
            set { AIcono = value; }
        }


        public string Accion
        {
            get { return AAccion; }
            set { AAccion = value; }
        }


        public string Controlador
        {
            get { return AControlador; }
            set { AControlador = value; }
        }


        public DateTime FechaAlta
        {
            get { return AFechaAlta; }
            set { AFechaAlta = value; }
        }


        public int? mnuId
        {
            get { return AmnuId; }
            set { AmnuId = value; }
        }


        public bool Activo
        {
            get { return AActivo; }
            set { AActivo = value; }
        }

        #endregion Propiedaddes - Get/Set

    }
}
