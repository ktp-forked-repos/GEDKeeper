﻿/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2019 by Sergey V. Zhdanovskih.
 *
 *  This file is part of "GEDKeeper".
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using BSLib;
using GKCommon.GEDCOM;
using NUnit.Framework;

namespace GKCommon.GEDCOM
{
    [TestFixture]
    public class GEDCOMTagTests
    {
        [Test]
        public void Test_SetTagStringsA()
        {
            var tag = new GEDCOMTag(null, "TEST", "");
            Assert.IsNotNull(tag);

            // very long string, 248"A" and " BBB BBBB"
            var strings = new string[] { "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA BBB BBBB" };
            GEDCOMTag.SetTagStrings(tag, strings);

            Assert.AreEqual(248, tag.StringValue.Length);

            var strList = GEDCOMTag.GetTagStrings(tag);
            Assert.IsNotNull(strList);
            Assert.AreEqual(1, strList.Count);
            Assert.AreEqual(strings[0], strList.Text);
        }

        [Test]
        public void Test_SetTagStringsL()
        {
            var tag = new GEDCOMTag(null, "TEST", "");
            Assert.IsNotNull(tag);

            // very long string, 248"A" and " BBB BBBB"
            var strings = new StringList( "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA BBB BBBB" );

            GEDCOMTag.SetTagStrings(null, strings);

            GEDCOMTag.SetTagStrings(tag, strings);

            Assert.AreEqual(248, tag.StringValue.Length);

            var strList = GEDCOMTag.GetTagStrings(tag);
            Assert.IsNotNull(strList);
            Assert.AreEqual(1, strList.Count);
            Assert.AreEqual(strings.Text, strList.Text);
        }

        [Test]
        public void Test_FindTags()
        {
            var tag = new GEDCOMTag(null, "TEST", "");
            Assert.IsNotNull(tag);

            tag.AddTag(GEDCOMTagType._FOLDER, "Private", null);
            tag.AddTag(GEDCOMTagType._FOLDER, "Friends", null);
            tag.AddTag(GEDCOMTagType._FOLDER, "Research", null);

            var subTags = tag.FindTags(GEDCOMTagType._FOLDER);
            Assert.AreEqual(3, subTags.Count);

            tag.DeleteTag(GEDCOMTagType._FOLDER);

            subTags = tag.FindTags(GEDCOMTagType._FOLDER);
            Assert.AreEqual(0, subTags.Count);
        }
    }
}
