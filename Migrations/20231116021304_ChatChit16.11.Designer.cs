﻿// <auto-generated />
using System;
using ChatChit.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChatChit.Migrations
{
    [DbContext(typeof(ChatChitContext))]
    [Migration("20231116021304_ChatChit16.11")]
    partial class ChatChit1611
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.24")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ChatChit.Models.FriendModel", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("friendId")
                        .HasColumnType("uuid")
                        .HasColumnName("friend_id");

                    b.Property<int?>("status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<Guid>("userId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("id");

                    b.HasIndex("friendId");

                    b.HasIndex("userId");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("ChatChit.Models.UserModel", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("fullName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("full_name");

                    b.Property<string>("image")
                        .HasColumnType("text")
                        .HasColumnName("image");

                    b.Property<string>("nickName")
                        .HasColumnType("text")
                        .HasColumnName("nick_name");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<DateTime>("updatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChatChit.Models.FriendModel", b =>
                {
                    b.HasOne("ChatChit.Models.UserModel", "Friend")
                        .WithMany()
                        .HasForeignKey("friendId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatChit.Models.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Friend");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
