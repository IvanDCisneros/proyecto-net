using Dominio.Clientes;
using Dominio.Contabilidades;
using Dominio.Inventario;
using Dominio.Notificaciones;
using Dominio.Parametrizacion;
using Dominio.Reportes;
using Dominio.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace Persistencia
{
    public class AplicationDBContext : DbContext
    {
        public AplicationDBContext(DbContextOptions<AplicationDBContext> options) : base(options)
        {

        }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<ProductoImagenes> ProductoImagenes { get; set; }
        public DbSet<ProductoMercancia> ProductoMercancia { get; set; }
        public DbSet<Talla> Talla { get; set; }
        public DbSet<Dominio.Parametrizacion.Color> Color { get; set; }
        public DbSet<Genero> Genero { get; set; }
        public DbSet<Banco> Bancos { get; set; }
        public DbSet<Departamentos> Departamentos { get; set; }
        public DbSet<Municipios> Municipios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Factura> Factura { get; set; }
        public DbSet<ItemFactura> ItemsFactura { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<NotificacionCompra> NotificacionCompra { get; set; }
        public DbSet<ReporteFactura> ReporteFactura { get; set; }
        public DbSet<EstadoPedido> EstadoPedidos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PlayList> PlayList { get; set; }

    }
}
