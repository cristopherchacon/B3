﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using B3.Clases;
using Oracle.DataAccess.Client;

namespace B3.Interfaz
{
    public partial class BajaLibro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                query name = new query();
               if(!IsPostBack)
                {
                ddlNombre.DataSource = name.querydt("select * from tabla_Libro");
                ddlNombre.DataTextField = "Titulo";
                ddlNombre.DataValueField = "ISBM";
                ddlNombre.DataBind();
               }
               gvBuscar.DataSource = name.querydt("select titulo, 'activo' as Estado from tabla_Libro where activo_libro=1 union select titulo, 'inactivo' as Estado from tabla_Libro where activo_libro=0");
               gvBuscar.DataBind();
            }
            catch (Exception ex)
            {
                msgError.Text = ex.Message;
                imgError.Visible = true;
            }
            
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Interfaz/Default.aspx");
            Limpiar();
        }

        protected void Limpiar()
        {
            imgError.Visible = false;
        }

        protected void ConfiguracionTipo(String text)
        {
                try
             {
                 query name = new query();
                 OracleConnection con = new OracleConnection(name.OracleConnString());
            OracleCommand cmd = new OracleCommand(text, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ISBM", OracleDbType.Int32).Value = ddlNombre.SelectedValue;
            con.Open();
            OracleDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            dr.Close();
            con.Close();
            Response.Redirect("~/Interfaz/BajaLibro.aspx");
             }
            catch (Exception ex)
            {
                msgError.Text = ex.Message;
            }
            if (msgError.Text.Equals(""))
            {
                Limpiar();
            }
            else
            {
                imgError.Visible = true;
            }
        }

        protected void btnAlta_Click(object sender, EventArgs e)
        {
            ConfiguracionTipo("activar_libro");
        }

        protected void btnBaja_Click(object sender, EventArgs e)
        {
            ConfiguracionTipo("desac_libro");
        }
    }
}