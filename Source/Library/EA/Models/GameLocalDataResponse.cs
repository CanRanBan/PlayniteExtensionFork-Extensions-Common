﻿using System.Collections.Generic;

namespace OriginLibrary.Models
{
    public class GameLocalDataResponse
    {
        public class LocalizableAttributes
        {
            public string longDescription;
            public string displayName;
        }

        public class I18n
        {
            public string longDescription;
            public string displayName;
        }

        public class Publishing
        {
            public class Software
            {
                public class FulfillmentAttributes
                {
                    public string executePathOverride;
                    public string installationDirectory;
                    public string installCheckOverride;
                }

                public string softwareId;
                public string softwarePlatform;
                public FulfillmentAttributes fulfillmentAttributes;
            }

            public class SoftwareList
            {
                public List<Software> software;
            }

            public SoftwareList softwareList;
        }

        public string offerId;
        public string offerType;
        public Publishing publishing;
        public LocalizableAttributes localizableAttributes;
        public I18n i18n;
        public string itemName;
    }
}
