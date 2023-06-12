﻿using Entiteti;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CGMV
{
    public partial class DodajPredavaca : Form
    {
        private const string PATH = "ListaPredavaca.txt";
        //private const char DEL = '|';
        List<Osoba> predavaci = new();

        public DodajPredavaca()
        {
            InitializeComponent();
        }

        private void BtnNatrag_Click(object sender, EventArgs e)
        {
            Administrator_Pocetna adminPocetna = new();
            adminPocetna.Show();
            adminPocetna.BringToFront();
            this.Close();
        }

        private void BtnDodajPredavača_Click(object sender, EventArgs e)
        {
            Osoba osobaa = new Osoba();
            string[] file = File.ReadAllLines(PATH);
            try
            {
                foreach (string line in file)
                {
                    osobaa = Osoba.ParseFromFileLine(line);
                    predavaci.Add(osobaa);
                }
            }
            catch (Exception em)
            {
                MessageBox.Show(em.Message);
            }
            if (!formValid())
            {
                return;
            }

            if (!userexists())
            {
                return;
            }

            Osoba osoba = new()
            {
                Ime = tbImePredavaca.Text,
                Prezime = tbPrezimePredavaca.Text,
                Email = tbEmailPredavaca.Text,
                Lozinka = tbLozinka.Text,
                JeAdmin = false
            };
            predavaci.Add(osoba);
            try
            {
                File.WriteAllLines(PATH, predavaci.Select(p => p.FormatForFileLine()));
                this.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show("Predavač uspješno dodan!");

            Administrator_Pocetna adminPocetna = new();
            adminPocetna.Show();
            adminPocetna.BringToFront();
            this.Close();
        }

        private bool userexists()
        {
            List<Osoba> predavaci = new List<Osoba>();
            string[] detalji;
            try
            {
                detalji = File.ReadAllLines(PATH);
                foreach (var item in detalji)
                {
                    predavaci.Add(Osoba.ParseFromFileLine(item));
                }
                foreach (var item in predavaci)
                {
                    if (item.Ime == tbImePredavaca.Text && item.Prezime == tbPrezimePredavaca.Text)
                    {
                        MessageBox.Show("Predavač sa tim imenom i prezimenom već postoji, nemoguć unos!");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ew)
            {
                MessageBox.Show(ew.Message);
                return false;
            }
        }

        private bool formValid()
        {
            if (String.IsNullOrEmpty(tbImePredavaca.Text) || String.IsNullOrEmpty(tbPrezimePredavaca.Text) ||
                String.IsNullOrEmpty(tbEmailPredavaca.Text) || String.IsNullOrEmpty(tbLozinka.Text) ||
                String.IsNullOrEmpty(tbPotvrdiLozinku.Text))
            {
                MessageBox.Show("Molimo unesite sve potrebne informacije");
                return false;
            }
            if (tbLozinka.Text != tbPotvrdiLozinku.Text)
            {
                MessageBox.Show("Greška prilikom kreiranja lozinke");
                return false;
            }
            return true;
        }


    }
}
