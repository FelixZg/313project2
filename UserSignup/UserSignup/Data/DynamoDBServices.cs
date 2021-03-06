﻿using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserSignup.Models;

namespace UserSignup.Data
{
    public class DynamoDBServices
    {
        IAmazonDynamoDB dynamoDBClient { get; set; }

        public DynamoDBServices(IAmazonDynamoDB dynamoDBClient)
        {
            this.dynamoDBClient = dynamoDBClient;
        }

        public async Task<User> InsertUser(User user)
        {
            DynamoDBContext context = new DynamoDBContext(dynamoDBClient);
            // Add a unique id for the primary key.
            //user.Id = System.Guid.NewGuid().ToString();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            User newUser = new User();
            newUser.Username = user.Username;
            newUser.Password = hashedPassword;
            await context.SaveAsync(newUser, default);
            User returnedUser = await context.LoadAsync<User>(newUser.Username, default);
            return returnedUser;
        }

        public async Task<User> GetUserAsync(string username)
        {
            DynamoDBContext context = new DynamoDBContext(dynamoDBClient);
            User newUser = await context.LoadAsync<User>(username, default(System.Threading.CancellationToken));
            return newUser;
        }

        public async Task<User> UpdateUser(User user)
        {
            DynamoDBContext context = new DynamoDBContext(dynamoDBClient);
            await context.SaveAsync(user, default(System.Threading.CancellationToken));
            User updatedUser = await context.LoadAsync<User>(user.Username, default(System.Threading.CancellationToken));
            return updatedUser;
        }

        public async Task<Card> getCard(string id)
        {
            DynamoDBContext context = new DynamoDBContext(dynamoDBClient);
            Card card = await context.LoadAsync<Card>(id, default(System.Threading.CancellationToken));
            return card;
        }

        public async Task<Card> InsertCard(Card card)
        {
            DynamoDBContext context = new DynamoDBContext(dynamoDBClient);
            card.Id = Guid.NewGuid().ToString();
            await context.SaveAsync(card, default);
            Card newCard = await context.LoadAsync<Card>(card.Id, default);
            return newCard;
        }

        public async Task<Card> UpdateCard(Card card)
        {
            DynamoDBContext context = new DynamoDBContext(dynamoDBClient);
            await context.SaveAsync(card, default(System.Threading.CancellationToken));
            Card newCard = await context.LoadAsync<Card>(card.Id, default(System.Threading.CancellationToken));
            return newCard;
        }

        public async void DeleteCard(string id)
        {
            DynamoDBContext context = new DynamoDBContext(dynamoDBClient);
            await context.DeleteAsync<Card>(id, default(System.Threading.CancellationToken));
        }
    }
}
