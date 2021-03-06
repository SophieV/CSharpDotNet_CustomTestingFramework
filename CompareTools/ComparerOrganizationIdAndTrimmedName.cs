﻿using System;
using System.Collections.Generic;

namespace TestMVC4App.Models
{
    public class ComparerOrganizationIdAndTrimmedName : IEqualityComparer<OrganizationTreeDescriptor>
    {
        bool IEqualityComparer<OrganizationTreeDescriptor>.Equals(OrganizationTreeDescriptor x, OrganizationTreeDescriptor y)
        {
            bool areEqual = false;

            // if IDs match
            if (x.ID == y.ID)
            {
                areEqual = true;

                if (x.Depth == y.Depth)
                {
                    x.HasBeenMatched = true;
                    x.MatchedPartner = y;
                    y.HasBeenMatched = true;
                    y.MatchedPartner = x;
                }
            }

            // if no ID on the old side and names match
            if (string.IsNullOrEmpty(y.ID))
            {
                string oldString = string.Empty;
                string newString = string.Empty;

                try
                {
                    oldString = y.Name.Trim();
                }
                catch (Exception) { }

                try
                {
                    newString = x.Name.Trim();
                }
                catch (Exception) { }

                if (oldString == newString)
                {
                    areEqual = true;

                    if (x.Depth == y.Depth)
                    {
                        x.HasBeenMatched = true;
                        x.MatchedPartner = y;
                        y.HasBeenMatched = true;
                        y.MatchedPartner = x;
                    }
                }
            }

            return areEqual;
        }

        int IEqualityComparer<OrganizationTreeDescriptor>.GetHashCode(OrganizationTreeDescriptor obj)
        {
            return obj.ToString().ToLower().GetHashCode();
        }
    }
}