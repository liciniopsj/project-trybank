﻿namespace trybank;

public class Trybank
{
    public bool Logged;
    public int loggedUser;

    //0 -> Número da conta
    //1 -> Agência
    //2 -> Senha
    //3 -> Saldo
    public int[,] Bank;
    public int registeredAccounts;
    private int maxAccounts = 50;
    public Trybank()
    {
        loggedUser = -99;
        registeredAccounts = 0;
        Logged = false;
        Bank = new int[maxAccounts, 4];
    }

    // 1. Construa a funcionalidade de cadastrar novas contas
    public void RegisterAccount(int number, int agency, int pass)
    {
        if (IsAccountInUse(number, agency))
        {
            throw new ArgumentException("A conta já está sendo usada!");
        }

        Bank[registeredAccounts, 0] = number;
        Bank[registeredAccounts, 1] = agency;
        Bank[registeredAccounts, 2] = pass;
        Bank[registeredAccounts, 3] = 0;
        registeredAccounts++;

    }

    private bool IsAccountInUse(int number, int agency)
    {
        for (int i = 0; i < registeredAccounts; i++)
        {
            if (Bank[i, 0] == number && Bank[i, 1] == agency)
            {
                return true;
            }
        }
        return false;
    }

    private void LoggedStatusCheck()
    {
        if (!Logged) throw new AccessViolationException("Usuário não está logado");
    }

    // 2. Construa a funcionalidade de fazer Login
    public void Login(int number, int agency, int pass)
    {
        string accountKey = $"{number} + {agency}";
        if (Logged)
        {
            throw new AccessViolationException("Usuário já está logado");
        }

        for (int i = 0; i < registeredAccounts; i++)
        {
            string registeredAccountsKey = $"{Bank[i, 0]} + {Bank[i, 1]}";
            if (accountKey == registeredAccountsKey)
            {

                if (Bank[i, 2] == pass)
                {
                    Logged = true;
                    loggedUser = i;
                    return;
                }
                else
                {
                    throw new ArgumentException("Senha incorreta");
                }
            }
        }
        throw new ArgumentException("Agência + Conta não encontrada");
    }

    // 3. Construa a funcionalidade de fazer Logout
    public void Logout()
    {
        LoggedStatusCheck();
        Logged = false;
        loggedUser = -99;
    }

    // 4. Construa a funcionalidade de checar o saldo
    public int CheckBalance()
    {
        LoggedStatusCheck();
        int balance = Bank[loggedUser, 3];
        return balance;

    }

    // 5. Construa a funcionalidade de depositar dinheiro
    public void Deposit(int value)
    {
        LoggedStatusCheck();
        Bank[loggedUser, 3] += value;
    }

    // 6. Construa a funcionalidade de sacar dinheiro
    public void Withdraw(int value)
    {
        LoggedStatusCheck();
        if (Bank[loggedUser, 3] > value)
        {
            Bank[loggedUser, 3] -= value;
        }
        else
        {
            throw new InvalidOperationException("Saldo insuficiente");
        }
    }

    // 7. Construa a funcionalidade de transferir dinheiro entre contas
    public void Transfer(int destinationNumber, int destinationAgency, int value)
    {
        LoggedStatusCheck();
        Withdraw(value);
        for (int i = 0; i < registeredAccounts; i++)
        {
            if (Bank[i, 0] == destinationNumber && Bank[i, 1] == destinationAgency)
            {
                Bank[i, 3] += value;
                return;
            }
        }
    }


}
