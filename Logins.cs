using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace bioAdventure
{
    public struct Logins
    {
        public string Nome;
        public string UserName;
        public string Senha;
        public string Email;
        public int Codigo;
    }
        public class Login
    {
        static Logins[] usuario = new Logins[999];

        //cadastro
        public static void cadastro()
        {
        int y;       
        
        do{
                usuario[Program.contador].Codigo = Program.contador;
                Console.WriteLine("Digite seu Nome: ");
                usuario[Program.contador].Nome = Console.ReadLine();
                Console.WriteLine("Digite seu Email: ");
                usuario[Program.contador].Email = Console.ReadLine();
                Console.WriteLine("Digite seu Login: ");
                usuario[Program.contador].UserName = Console.ReadLine();

            int i = 0;
            do{    
                Console.WriteLine("Digite seu Senha: ");
                usuario[Program.contador].Senha = Console.ReadLine();
                Console.WriteLine("Confirme sua senha: ");
                if(Console.ReadLine() != usuario[Program.contador].Senha)
                {
                    Console.WriteLine("\nAs senhas devem ser identicas. ");
                    i = 1;
                } else i = 0;
            } while(i != 0); 

            Console.WriteLine("Deseja cadastrar mais usuarios? \nS - Sim\nN - Não");
            if(Console.ReadLine() == "s"){
                y = 0;
            } else y = 1;
            } while(y == 0);
        }        
        //logar
        public static void logar()
        {
            
        }
        
    }
}



