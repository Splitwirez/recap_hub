using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ReCap.Hub.Models
{
    public class CreatureModelRefModel : CreatureModelBase
    {
        public override void RefreshFromXml(XElement element, bool notify)
        {
            base.RefreshFromXml(element, notify);
        }

        public override void SaveToXml(ref XElement element)
        {
            try
            {
                Creature.SaveToXml(ref element);
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine("CreatureModelRefModel.SaveToXml threw NullReferenceException:\n\n" + ex.ToString());
                base.SaveToXml(ref element);
            }
        }

        CreatureModel _creature = null;
        public CreatureModel Creature
        {
            get => _creature;
            protected set => RASIC(ref _creature, value);
        }



        public CreatureModelRefModel()
            : base()
        {
        }
        public CreatureModelRefModel(CreatureModel creatureModel)
            : this()
        {
            Creature = creatureModel;
            /*CreatureModel creature = creatureModel;
            if (TryGetNonRefCreature(ref creatureModel, out CreatureModel nonRef))
            {
                Creature = nonRef;
            }*/
        }

        /*public static bool TryGetNonRefCreature(ref CreatureModel creatureModel, out CreatureModel nonRef)
        {
            nonRef = null;
            if (creatureModel == null)
                return false;
            
            
            if (!(creatureModel is CreatureModelRefModel))
                nonRef = creatureModel;
            else if
                (
                    (creatureModel != null)
                    && (creatureModel is CreatureModelRefModel refModel)
                    && TryGetRootRef(ref refModel, out CreatureModelRefModel rootRef)
                )
            {
                //if (rootRef is CreatureModelRefModel)
                    nonRef = rootRef.Creature;
                /*else
                    nonRef = rootRef;* /
            }

#if DEBUG
            if ((nonRef != null) && (nonRef is CreatureModelRefModel))
                throw new Exception($"'{nameof(nonRef)}' is of type CreatureModelRefModel, which it should never be");
#endif

            return nonRef != null;
        }
        public static bool TryGetRootRef(ref CreatureModelRefModel creatureModel, out CreatureModelRefModel rootRef)
        {
            CreatureModel creature = creatureModel;
            while ((creature != null) && (creature is CreatureModelRefModel creatureRef))
            {
                creature = creatureRef.Creature;
            }

            rootRef = (creature is CreatureModelRefModel creatureRefModel)
                ? creatureRefModel
                : null
            ;

            return rootRef != null;
        }*/

        
        public bool TryEnsureCreature(IEnumerable<CreatureModel> creatures)
        {
            var strictMatch = creatures.FirstOrDefault(x => IsMe(x, true));
            CreatureModel matchedCreature = null;
            if (strictMatch != null)
            {
                matchedCreature = strictMatch;
            }
            else
            {
                var match = creatures.FirstOrDefault(x => IsMe(x, false));
                if (match != null)
                {
                    matchedCreature = match;
                }
            }

            Creature = matchedCreature;
            return matchedCreature != null;
        }

        bool IsMe(CreatureModelBase creature, bool strict)
        {
            bool matchedNounID = NounID == creature.NounID;
            if (strict)
            {
                bool matchedID = ID == creature.ID;

                return
                (
                    matchedNounID
                    && matchedID
                );
            }
            


            return
            (
                matchedNounID
            );
        }
    }
}