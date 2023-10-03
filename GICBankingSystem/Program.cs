using GICBankingSystem;
using GICBankingSystem.Data;
using GICBankingSystem.DTOs;
using GICBankingSystem.Entities;
using GICBankingSystem.Gic.Core;
using GICBankingSystem.Gic.Services;
using GICBankingSystem.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Globalization;
using System.Numerics;
using System.Security.Principal;
using System.Threading.Tasks;
using static GICBankingSystem.Utils.Enum;

Console.Title = "GIC Banking System";

var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

var host = CreateHostBuilder(args, configuration).Build();

static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<DataContext>(options =>
                        options.UseSqlite(hostContext.Configuration.GetConnectionString("Sqlite")));
                services.AddTransient<ITransactionService, TransactionService>();
                services.AddTransient<IAccountService, AccountService>();
                services.AddTransient<IRuleService, RuleService>();
                services.AddTransient<IPrintStatementService, PrintStatementService>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders(); 
            });

var transactionService = host.Services.GetRequiredService<ITransactionService>();
var accountService = host.Services.GetRequiredService<IAccountService>();
var ruleService = host.Services.GetRequiredService<IRuleService>();
var printService = host.Services.GetRequiredService<IPrintStatementService>();


try
{
    bool isInitialRound = true;
    do
    { 
        if (isInitialRound)
        {
            isInitialRound = false;
            Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");
        }
        else
        {
            Console.WriteLine("Is there anything else you'd like to do?");
        }
        
        Console.WriteLine("[T] Input transactions");
        Console.WriteLine("[I] Define interest rules");
        Console.WriteLine("[P] Print statement");
        Console.WriteLine("[Q] Quit");
        var input = Console.ReadLine();

        switch (input)
        {
            case "T":
            case "t":
                Console.WriteLine("Please enter transaction details in <Date> <Account> <Type> <Amount> format");
                Console.WriteLine("(or enter blank to go back to main menu):");
                var transactionInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(transactionInput))
                {
                    break;
                }
                await HandleTransaction(transactionInput);
                break;
            case "I":
            case "i": 
                Console.WriteLine("Please enter interest rules details in <Date> <RuleId> <Rate in %> format");
                Console.WriteLine("(or enter blank to go back to main menu)");
                var ruleInput = Console.ReadLine();
                await HandleRule(ruleInput);
                break;
            case "P":
            case "p":
                Console.WriteLine("Please enter account and month to generate the statement<Account> <Year> <Month>");
                Console.WriteLine("(or enter blank to go back to main menu)");
                var printStatementInput = Console.ReadLine();
                await HandlePrintStatement(printStatementInput);
                break;
            case "Q":
            case "q":
                Console.WriteLine("Thank you for banking with AwesomeGIC Bank.");
                Console.WriteLine("Have a nice day!");                

                Environment.Exit(0);
                break;

        }

    } while (true);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}


async Task HandleTransaction(string? transactionInput)
{
    if (transactionInput is null)
    {
        Console.WriteLine("Invalid input strng");
        return;
    }

    var transaction = ParseTransaction(transactionInput);
     
    await transactionService.AddTransactionAsync(transaction);
     
}

TransactionDTO ParseTransaction(string transactionInput)
{
    TransactionDTO transaction = new();

    var transactionArr = transactionInput.Split(' ');
     
    transaction.Date = DateTime.ParseExact(transactionArr[0], "yyyyMMdd", CultureInfo.InvariantCulture);
    transaction.AccountNumber = transactionArr[1];
    transaction.Type = transactionArr[2];
    transaction.Amount = decimal.Parse(transactionArr[3]);

    return transaction;
}

async Task HandleRule(string? ruleInput)
{
    if (ruleInput is null)
    {
        Console.WriteLine("Invalid input strng");
        return;
    }
    var rule = ParseRule(ruleInput);
     
    await ruleService.AddRuleAsync(rule);
       
}

RuleDTO ParseRule(string ruleInput)
{
    RuleDTO rule = new();

    var ruleArr = ruleInput.Split(' ');
    rule.Date = DateTime.ParseExact(ruleArr[0], "yyyyMMdd", CultureInfo.InvariantCulture);
    rule.RuleID = ruleArr[1];
    rule.Rate = Convert.ToSingle(ruleArr[2]); 

    return rule;
}

async Task HandlePrintStatement(string? printStatementInput)
{
    if(printStatementInput is null)
    {
        Console.WriteLine("Invalid input strng");
        return;
    }
    var printSatement = ParsePrintStatement(printStatementInput);
    await printService.GeneratePrintStatement(printSatement);
}

PrinStatementDTO ParsePrintStatement(string printInput)
{
    PrinStatementDTO prinStatement = new();

    var printStatementArr = printInput.Split(' ');
    prinStatement.Account = printStatementArr[0];
    prinStatement.Year = Convert.ToInt32(printStatementArr[1]);
    prinStatement.Month = Convert.ToInt32(printStatementArr[2]);

    return prinStatement;
}
