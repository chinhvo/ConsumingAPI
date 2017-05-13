using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Mvc.Routing;

namespace ServerAPI.Controllers
{
    public class ProductsController : ApiController
    {
        private NORTHWNDEntities db = new NORTHWNDEntities();

        // GET api/Products
        [HttpGet]
        [Route("api/Products")]
        public HttpResponseMessage GetProducts()
        {
            Mapper.CreateMap<Product, ProductDTO>();
            var tempData = db.Products.ToList();
            var products = Mapper.Map<List<Product>, List<ProductDTO>>(tempData);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, products);
            return response;
        }

        // GET api/Products/5
        public Product GetProduct(int id)
        {
            Product product = db.Products.Single(p => p.ProductID == id);
            if (product == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return product;
        }

        // PUT api/Products/5
        public HttpResponseMessage PutProduct(int id, Product product)
        {
            if (ModelState.IsValid && id == product.ProductID)
            {
                db.Products.Attach(product);
                db.ObjectStateManager.ChangeObjectState(product, EntityState.Modified);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/Products
        [HttpPost]
        [Route("api/Products")]
        public HttpResponseMessage PostProduct([FromBody] ProductDTO productDto)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<ProductDTO, Product>();
                var product = Mapper.Map<ProductDTO, Product>(productDto);
                db.Products.AddObject(product);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, product);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = product.ProductID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Products/5
        public HttpResponseMessage DeleteProduct(int id)
        {
            Product product = db.Products.Single(p => p.ProductID == id);
            if (product == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Products.DeleteObject(product);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, product);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}