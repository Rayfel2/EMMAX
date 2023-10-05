using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoCore.Models;

public partial class TiendaPruebaContext : DbContext
{
    public TiendaPruebaContext()
    {
    }

    public TiendaPruebaContext(DbContextOptions<TiendaPruebaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Carrito> Carritos { get; set; }

    public virtual DbSet<CarritoProducto> CarritoProductos { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<ListaDeseo> ListaDeseos { get; set; }

    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Recibo> Recibos { get; set; }

    public virtual DbSet<Reseña> Reseñas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<ListaProducto> ListaProducto { get; set; }
    public virtual DbSet<RolesUsuario> RolesUsuario { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carrito>(entity =>
        {
            entity.HasKey(e => e.IdCarrito).HasName("PK__Carrito__195A55BF7E235BE4");

            entity.ToTable("Carrito");

            entity.Property(e => e.IdCarrito).HasColumnName("Id_carrito");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

            entity.HasOne(d => d.oUsuario).WithMany(p => p.Carritos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Carrito__Id_Usua__4316F928");
        });

        modelBuilder.Entity<CarritoProducto>(entity =>
        {
            entity.HasKey(e => new { e.IdCarrito, e.IdProducto }).HasName("PK__Carrito___1882BA4FA452C27B");

            entity.ToTable("Carrito_productos");

            entity.Property(e => e.IdCarrito)
                //.ValueGeneratedOnAdd()
                .HasColumnName("Id_carrito");
            entity.Property(e => e.IdProducto).HasColumnName("Id_producto");

            entity.HasOne(d => d.oCarrito).WithMany(p => p.CarritoProductos)
                .HasForeignKey(d => d.IdCarrito)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Carrito_p__Id_ca__4CA06362");

            entity.HasOne(d => d.oProducto).WithMany(p => p.CarritoProductos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Carrito_p__Id_pr__4D94879B");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__CB90334942F4F8AF");

            entity.Property(e => e.IdCategoria).HasColumnName("Id_Categoria");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(150)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<ListaDeseo>(entity =>
        {
            entity.HasKey(e => e.IdLista).HasName("PK__Lista_de__FD5DC8F4B84E07AD");

            entity.ToTable("Lista_deseos");

            entity.Property(e => e.IdLista).HasColumnName("Id_Lista");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

            entity.HasOne(d => d.oUsuario).WithMany(p => p.ListaDeseos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Lista_des__Id_Us__5070F446");

            entity.HasMany(d => d.IdProductos).WithMany(p => p.IdLista)
                .UsingEntity<Dictionary<string, object>>(
                    "ListaProducto",
                    r => r.HasOne<Producto>().WithMany()
                        .HasForeignKey("IdProductos")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Lista_Pro__Id_Pr__5441852A"),
                    l => l.HasOne<ListaDeseo>().WithMany()
                        .HasForeignKey("IdLista")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Lista_Pro__Id_Li__534D60F1"),
                    j =>
                    {
                        j.HasKey("IdLista", "IdProductos").HasName("PK__Lista_Pr__7B37C322EAD40186");
                        j.ToTable("Lista_Producto");
                        j.IndexerProperty<int>("IdLista")
                            .ValueGeneratedOnAdd()
                            .HasColumnName("Id_Lista");
                        j.IndexerProperty<int>("IdProductos").HasColumnName("Id_Productos");
                    });
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.IdMetodo).HasName("PK__Metodo_p__AB62E2F251AC33E5");

            entity.ToTable("Metodo_pago");

            entity.Property(e => e.IdMetodo).HasColumnName("Id_metodo");
            entity.Property(e => e.TipoMetodo)
                .HasMaxLength(50)
                .HasColumnName("tipo_metodo");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__1D8EFF01E2719CCB");

            entity.Property(e => e.IdProducto).HasColumnName("Id_producto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(150)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdCategoria).HasColumnName("Id_categoria");
            entity.Property(e => e.Imagen).HasColumnType("image");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio).HasColumnName("precio");
            entity.Property(e => e.Stock).HasColumnName("stock");

            entity.HasOne(d => d.oCategorium).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("FK__Productos__Id_ca__49C3F6B7");
        });

        modelBuilder.Entity<Recibo>(entity =>
        {
            entity.HasKey(e => e.IdRecibo).HasName("PK__Recibo__3854E199AA3E8DBE");

            entity.ToTable("Recibo");

            entity.Property(e => e.IdRecibo).HasColumnName("Id_Recibo");
            entity.Property(e => e.IdCarrito).HasColumnName("Id_Carrito");
            entity.Property(e => e.IdMetodoPago).HasColumnName("Id_metodo_pago");
            entity.Property(e => e.Impuestos).HasColumnName("impuestos");
            entity.Property(e => e.Subtotal).HasColumnName("subtotal");

            entity.HasOne(d => d.oCarrito).WithMany(p => p.Recibos)
                .HasForeignKey(d => d.IdCarrito)
                .HasConstraintName("FK__Recibo__Id_Carri__46E78A0C");

            entity.HasOne(d => d.oMetodoPago).WithMany(p => p.Recibos)
                .HasForeignKey(d => d.IdMetodoPago)
                .HasConstraintName("FK__Recibo__Id_metod__45F365D3");
        });

        modelBuilder.Entity<Reseña>(entity =>
        {
            entity.HasKey(e => e.IdReseña).HasName("PK__Reseña__D2EBE05CC1157BD1");

            entity.ToTable("Reseña");

            entity.Property(e => e.IdReseña).HasColumnName("Id_Reseña");
            entity.Property(e => e.Comentario)
                .HasMaxLength(150)
                .HasColumnName("comentario");
            entity.Property(e => e.IdProducto).HasColumnName("Id_producto");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
            entity.Property(e => e.ValorReseña).HasColumnName("valor_reseña");

            entity.HasOne(d => d.oProducto).WithMany(p => p.Reseñas)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__Reseña__Id_produ__5812160E");

            entity.HasOne(d => d.oUsuario).WithMany(p => p.Reseñas)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Reseña__Id_Usuar__571DF1D5");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRoles).HasName("PK__Roles__2610294B1830CE76");

            entity.Property(e => e.IdRoles).HasColumnName("Id_Roles");
            entity.Property(e => e.Rol).HasMaxLength(50);

            entity.HasMany(d => d.IdUsuarios).WithMany(p => p.IdRoles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolesUsuario",
                    r => r.HasOne<Usuario>().WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Roles_usu__Id_Us__403A8C7D"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("IdRoles")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Roles_usu__Id_Ro__3F466844"),
                    j =>
                    {
                        j.HasKey("IdRoles", "IdUsuario").HasName("PK__Roles_us__102C5FF567E63D1D");
                        j.ToTable("Roles_usuario");
                        j.IndexerProperty<int>("IdRoles")
                            .ValueGeneratedOnAdd()
                            .HasColumnName("Id_Roles");
                        j.IndexerProperty<int>("IdUsuario").HasColumnName("Id_Usuario");
                    });
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__DE4431C5E8A8D105");

            entity.ToTable("Usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("ID_Usuario");
            entity.Property(e => e.Apellido).HasMaxLength(55);
            entity.Property(e => e.Contraseña).HasMaxLength(50);
            entity.Property(e => e.Dirección).HasMaxLength(55);
            entity.Property(e => e.Email).HasMaxLength(55);
            entity.Property(e => e.FechaNacimiento)
                .HasColumnType("date")
                .HasColumnName("Fecha_Nacimiento");
            entity.Property(e => e.Nombre).HasMaxLength(55);
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(55)
                .HasColumnName("Nombre_Usuario");
            entity.Property(e => e.Teléfono).HasMaxLength(12);
        });


        modelBuilder.Entity<ListaProducto>(entity =>
        {
            entity.HasKey(e => new { e.IDListaProducto, e.IdProducto }).HasName("PK__ListaProducto___1882BA4FA452C27B");

            entity.ToTable("Lista_Productos");

            entity.Property(e => e.IDListaProducto)
                //.ValueGeneratedOnAdd()
                .HasColumnName("Id_Lista");
            entity.Property(e => e.IdProducto).HasColumnName("Id_producto");

            entity.HasOne(d => d.oListaDeseo).WithMany(p => p.IDListaProducto)
                .HasForeignKey(d => d.IDListaProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ListaProducto_p__Id_ca__4CA06362");

            entity.HasOne(d => d.oProducto).WithMany(p => p.IDListaProducto)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ListaProducto_p__Id_pr__4D94879B");
        });

        modelBuilder.Entity<RolesUsuario>(entity =>
        {
            entity.HasKey(e => new { e.IdRoles, e.IdUsuario }).HasName("PK__RoleUsuario___1882BA4FA452C27B");

            entity.ToTable("Roles_Usuario");

            entity.Property(e => e.IdRoles)
                //.ValueGeneratedOnAdd()
                .HasColumnName("Id_Roles");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

            entity.HasOne(d => d.oRole).WithMany(p => p.RolesUsuarios)
                .HasForeignKey(d => d.IdRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RolesUsuario_U__Id_ca__4CA06362");

            entity.HasOne(d => d.oUsuario).WithMany(p => p.RolesUsuarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RolesUsuario_p__Id_pr__4D94879B");
        });







        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
