// <copyright file="LocationLogic.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Esport.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Esport.Data;
    using Esport.Repository.Interfaces;
    using Esport.Repository;
    /// <summary>
    /// Location logic.
    /// </summary>
    public class LocationLogic : ILocationLogic
    {
        IRepository<Location> repo;

        public LocationLogic(IRepository<Location> repo)
        {
            this.repo = repo;
        }

        // UPDATE
        public void ChangeLocation(Location item)
        {
           this.repo.Update(item);
        }

        // LIST
        public IQueryable<Location> GetAllLocations()
        {
            return this.repo.GetAll();
        }

        // GetOne
        public Location GetLocationById(int id)
        {
            return this.repo.GetOne(id);
        }

        // CREATE
        public void InsertNewLocation(Location item)
        {
            this.repo.Insert(item);
        }

        // DELETE
        public void RemoveLocation(int id)
        {
            this.repo.Remove(id);
        }
    }
}
