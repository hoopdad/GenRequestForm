using ChatGPTRunner.Data;
using ChatGPTRunner.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

Console.WriteLine("Hello, World!");

var db = new ApplicationDbContext();

var requests = await db.GenRequest
    .Where(b => b.Status== "Requested")
    .ToListAsync();

foreach (GenRequest req in requests)
{
    Console.WriteLine(req.Title);
    Console.WriteLine(req.Actor);


}

