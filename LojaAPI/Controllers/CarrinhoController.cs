using Loja.DAO;
using Loja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LojaAPI.Controllers
{
    public class CarrinhoController : ApiController
    {
        public HttpResponseMessage Get(int id)
        {
            try
            {
                CarrinhoDAO dao = new CarrinhoDAO();
                Carrinho carrinho = dao.Busca(id);
                return Request.CreateResponse(HttpStatusCode.OK, carrinho);
            }
            catch (KeyNotFoundException)
            {
                string message = string.Format("O carrinho {0} não foi encontrado", id);
                HttpError error = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.NotFound, error);
            }

        }

        // forca ser o retorno em XML
        //public string Get(int id)
        //{
        //    CarrinhoDAO dao = new CarrinhoDAO();
        //    Carrinho carrinho = dao.Busca(id);

        //    return carrinho.ToXml();
        //}

        public HttpResponseMessage Post([FromBody]Carrinho carrinho)
        {
            CarrinhoDAO dao = new CarrinhoDAO();
            dao.Adiciona(carrinho);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            string location = Url.Link("DefaultApi", new { controller = "carrinho", id = carrinho.Id });
            response.Headers.Location = new Uri(location);

            return response;
        }

        //public string Post([FromBody]Carrinho carrinho)
        //{
        //    CarrinhoDAO dao = new CarrinhoDAO();
        //    dao.Adiciona(carrinho);
        //    return "Sucesso!";
        //}

        [Route("api/carrinho/{idCarrinho}/produto/{idProduto}")]
        public HttpResponseMessage Delete([FromUri]int idCarrinho, [FromUri]int idProduto)
        {
            CarrinhoDAO dao = new CarrinhoDAO();
            Carrinho carrinho = dao.Busca(idCarrinho);
            carrinho.Remove(idProduto);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/carrinho/{idCarrinho}/produto/{idProduto}/quantidade")]
        public HttpResponseMessage Put([FromBody]Produto produto, [FromUri]int idCarrinho, [FromUri]int idProduto)
        {
            CarrinhoDAO dao = new CarrinhoDAO();
            Carrinho carrinho = dao.Busca(idCarrinho);
            carrinho.TrocaQuantidade(produto);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}
