﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GeneticAlgorithmLogic.Сommon
{
    public class DataLoader
    {
        #region Private parameters

        #region Consts

        private const int CapacityOfKnapsackIndex = 4;

        private const int DimensionIndex = 2;

        private const int MaxSpeedIndex = 6;

        private const int MinSpeedIndex = 5;

        private const int NumberOfItemsIndex = 3;

        private const int PlacesStartIndex = 10;

        private const int RentingRatioIndex = 7;

        /// <summary>
        /// ValueIndex = 1; means that at in string[] numerical value is at ValueIndex position
        /// </summary>
        private const int ValueIndex = 1;

        #endregion Consts

        public int ItemsStartIndex => PlacesStartIndex + NumberOfPlaces + 1;
        public int NumberOfPlaces { get; set; }

        #region Item

        private const int ItemIdIndex = 0;
        public const int ItemProfitIndex = 1;
        public const int ItemWeightIndex = 2;
        public const int ItemLocationIndex = 3;

        #endregion Item

        #region Place

        public const int PlaceIdIndex = 0;
        private const int PlacePositionXIndex = 1;
        private const int PlacePositionYIndex = 2;

        #endregion Place

        #endregion Private parameters

        public DataContainer GetCreatedDataContainerFromFile(string filePath)
        {
            var allLines = File.ReadAllLines(filePath);

            return CreateDataContainer(allLines);
        }

        private DataContainer CreateDataContainer(string[] allLinesFromFile)
        {
            DataContainer dataContainer = new DataContainer
            {
                Dimension = GetIntFromLine(allLinesFromFile.ElementAt(DimensionIndex), ValueIndex),
                NumberOfItems = GetIntFromLine(allLinesFromFile.ElementAt(NumberOfItemsIndex), ValueIndex),
                MaxCapacityOfKnapsack = GetIntFromLine(allLinesFromFile.ElementAt(CapacityOfKnapsackIndex), ValueIndex),

                MinSpeed = GetDoubleFromLine(allLinesFromFile.ElementAt(MinSpeedIndex), ValueIndex),
                MaxSpeed = GetDoubleFromLine(allLinesFromFile.ElementAt(MaxSpeedIndex), ValueIndex),
                RentingRatio = GetDoubleFromLine(allLinesFromFile.ElementAt(RentingRatioIndex), ValueIndex),
            };

            var places = CreatePlaces(allLinesFromFile, PlacesStartIndex, dataContainer.Dimension);

            var items = CreateItems(allLinesFromFile, ItemsStartIndex, dataContainer.NumberOfItems);

            dataContainer.Places = places;
            dataContainer.DistanceMatrix = CreateDistanceMatrix(places);
            dataContainer.Items = items;

            return dataContainer;
        }

        private double[,] CreateDistanceMatrix(List<Place> places)
        {
            int placesCount = places.Count;
            double[,] distanceMatrix = new double[placesCount, placesCount];

            for (int i = 0; i < placesCount; i++)
            {
                for (int j = 0; j < placesCount; j++)
                {
                    distanceMatrix[i, j] = EuclideanDistance.FindEuclideanDistance(places.ElementAt(i), places.ElementAt(j));
                }
            }

            return distanceMatrix;
        }

        private Dictionary<int, Item> CreateItems(string[] allLinesFromFile, int firstItemIndex, int numberOfItems)
        {
            Dictionary<int, Item> items = new Dictionary<int, Item>(numberOfItems);
            var counter = numberOfItems + firstItemIndex;
            for (int i = firstItemIndex; i < counter; i++)
            {
                var line = allLinesFromFile.ElementAt(i);

                var itemId = GetIntFromLine(line, ItemIdIndex);
                var itemProfit = GetIntFromLine(line, ItemProfitIndex);
                var itemWeight = GetIntFromLine(line, ItemWeightIndex);
                var itemPlaceId = GetIntFromLine(line, ItemLocationIndex);

                #region id decreasing by 1 because at input file it id starts by index = 1. For this implementation necessary to start index by 0.

                itemId--;
                itemPlaceId--;

                #endregion id decreasing by 1 because at input file it id starts by index = 1. For this implementation necessary to start index by 0.

                Item item = new Item()
                {
                    Id = itemId,
                    Weight = itemWeight,
                    Profit = itemProfit,
                    PlaceId = itemPlaceId
                };

                items.Add(itemId, item);
            }

            return items;
        }

        private List<Place> CreatePlaces(string[] allLinesFromFile, int firstPlaceIndex, int numberOfPlaces)
        {
            List<Place> places = new List<Place>();
            var counter = numberOfPlaces + firstPlaceIndex;
            for (int i = firstPlaceIndex; i < counter; i++)
            {
                var line = allLinesFromFile.ElementAt(i);

                var placeId = GetIntFromLine(line, PlaceIdIndex);
                var placePositionX = GetDoubleFromLine(line, PlacePositionXIndex);
                var placePositionY = GetDoubleFromLine(line, PlacePositionYIndex);

                #region id decreasing by 1 because at input file it id starts by index = 1. For this implementation necessary to start index by 0.

                placeId--;

                #endregion id decreasing by 1 because at input file it id starts by index = 1. For this implementation necessary to start index by 0.

                Place tmpPlace = new Place()
                {
                    Id = placeId,
                    PozitionX = placePositionX,
                    PozitionY = placePositionY
                };

                places.Add(tmpPlace);
            }

            NumberOfPlaces = places.Count;
            return places;
        }

        private double GetDoubleFromLine(string line, int indexAtSplitedLine)
        {
            var splitedLine = line.Split("\t");

            return Double.Parse(splitedLine[indexAtSplitedLine]);
        }

        private int GetIntFromLine(string line, int indexAtSplitedLine)
        {
            var splitedLine = line.Split("\t");

            return Int32.Parse(splitedLine[indexAtSplitedLine]);
        }
    }
}