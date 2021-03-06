﻿/*
 * Copyright (c) 2017 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * http://github.com/piranhacms/piranha
 * 
 */

using Piranha.Extend;
using System;
using System.Linq;
using System.Text;
using Xunit;

namespace Piranha.Tests
{
    public class Fields : BaseTests
    {
        [Field(Name = "First", Shorthand = "1")]
        public class MyFirstField : Extend.Fields.SimpleField<string> { }

        [Field(Name = "Second")]
        public class MySecondField : Extend.Fields.SimpleField<string> { }

        [Field(Name = "Third")]
        public class MyThirdField : Extend.Fields.SimpleField<string> { }

        private FieldInfoList fields = new FieldInfoList();

        /// <summary>
        /// Sets up & initializes the tests.
        /// </summary>
        protected override void Init() {
            using (var api = new Api(GetDb(), storage)) {
                Piranha.App.Init(api);
                Piranha.App.Fields.Register<MyFirstField>();

                fields.Register<MyFirstField>();
                fields.Register<MyThirdField>();
            }
        }

        /// <summary>
        /// Cleans up any possible data and resources
        /// created by the test.
        /// </summary>
        protected override void Cleanup() { }

        [Fact]
        public void GetByType() {
            var field = Piranha.App.Fields.GetByType(typeof(Extend.Fields.HtmlField));

            Assert.NotNull(field);
            Assert.Equal("Html", field.Name);
        }

        [Fact]
        public void GetByTypeLocal() {
            var field = fields.GetByType(typeof(MyFirstField));

            Assert.NotNull(field);
            Assert.Equal("First", field.Name);
        }

        [Fact]
        public void GetByTypeName() {
            var field = Piranha.App.Fields.GetByType(typeof(Extend.Fields.HtmlField).FullName);

            Assert.NotNull(field);
            Assert.Equal("Html", field.Name);
        }

        [Fact]
        public void GetByTypeNameLocal() {
            var field = fields.GetByType(typeof(MyFirstField).FullName);

            Assert.NotNull(field);
            Assert.Equal("First", field.Name);
        }

        [Fact]
        public void GetByShorthand() {
            var field = Piranha.App.Fields.GetByShorthand("Html");

            Assert.NotNull(field);
            Assert.Equal("Html", field.Name);
        }

        [Fact]
        public void GetByShorthandLocal() {
            var field = fields.GetByShorthand("1");

            Assert.NotNull(field);
            Assert.Equal("First", field.Name);
        }

        [Fact]
        public void Register() {
            var count = Piranha.App.Fields.Count();

            Piranha.App.Fields.Register<MySecondField>();

            Assert.Equal(count + 1, Piranha.App.Fields.Count());
        }

        [Fact]
        public void RegisterLocal() {
            var count = fields.Count();

            fields.Register<MySecondField>();

            Assert.Equal(count + 1, fields.Count());
        }

        [Fact]
        public void UnRegister() {
            var count = Piranha.App.Fields.Count();

            Piranha.App.Fields.UnRegister<MyFirstField>();

            Assert.Equal(count - 1, Piranha.App.Fields.Count());
        }

        [Fact]
        public void UnRegisterLocal() {
            var count = fields.Count();

            fields.UnRegister<MyThirdField>();

            Assert.Equal(count - 1, fields.Count());
        }

        [Fact]
        public void Enumerate() {
            foreach (var field in Piranha.App.Fields) {
                Assert.NotNull(field);
            }
        }

        [Fact]
        public void EnumerateLocal() {
            foreach (var field in fields) {
                Assert.NotNull(field);
            }
        }

        [Fact]
        public void CheckBoxFieldConversions()
        {
            var value = true;

            Piranha.Extend.Fields.CheckBoxField field = value;
            Assert.Equal(value, field.Value);
        }

        [Fact]
        public void DateFieldConversions() {
            var now = DateTime.Now;

            Piranha.Extend.Fields.DateField field = now;
            Assert.Equal(now, field.Value);
        }

        [Fact]
        public void HtmlFieldConversions() {
            var str = "<p>This is a paragraph</p>";

            Piranha.Extend.Fields.HtmlField field = str;
            Assert.Equal(str, field.Value);

            string output = field;
            Assert.Equal(str, output);
        }

        [Fact]
        public void ImageFieldConversions() {
            var media = new Data.Media() {
                Id = Guid.NewGuid()
            };

            Piranha.Extend.Fields.ImageField field = media;
            Assert.Equal(media.Id, field.Id.Value);
        }

        [Fact]
        public void DocumentFieldConversions() {
            var media = new Data.Media() {
                Id = Guid.NewGuid()
            };

            Piranha.Extend.Fields.DocumentField field = media;
            Assert.Equal(media.Id, field.Id.Value);
        }

        [Fact]
        public void VideoFieldConversions() {
            var media = new Data.Media() {
                Id = Guid.NewGuid()
            };

            Piranha.Extend.Fields.VideoField field = media;
            Assert.Equal(media.Id, field.Id.Value);
        }

        [Fact]
        public void MediaFieldConversions() {
            var media = new Data.Media() {
                Id = Guid.NewGuid()
            };

            Piranha.Extend.Fields.MediaField field = media;
            Assert.Equal(media.Id, field.Id.Value);
        }

        [Fact]
        public void ImageFieldConversionsNullImage() {
            var id = Guid.NewGuid();

            Piranha.Extend.Fields.ImageField field = id;
            Assert.Equal(id, field.Id.Value);

            string url = field;
            Assert.Equal("", url);
        }

        [Fact]
        public void DocumentFieldConversionsNullDocument() {
            var id = Guid.NewGuid();

            Piranha.Extend.Fields.DocumentField field = id;
            Assert.Equal(id, field.Id.Value);

            string url = field;
            Assert.Equal("", url);
        }

        [Fact]
        public void VideoFieldConversionsNullVideo() {
            var id = Guid.NewGuid();

            Piranha.Extend.Fields.VideoField field = id;
            Assert.Equal(id, field.Id.Value);

            string url = field;
            Assert.Equal("", url);
        }

        [Fact]
        public void MediaFieldConversionsNullMedia() {
            var id = Guid.NewGuid();

            Piranha.Extend.Fields.MediaField field = id;
            Assert.Equal(id, field.Id.Value);

            string url = field;
            Assert.Equal("", url);
        }

        [Fact]
        public void MarkdownFieldConversions() {
            var md = "This is a paragraph";
            var html = Piranha.App.Markdown.Transform(md);

            Piranha.Extend.Fields.MarkdownField field = md;
            Assert.Equal(md, field.Value);

            string outputHtml = field;
            Assert.Equal(html, outputHtml);
        }

        [Fact]
        public void StringFieldConversions() {
            var inStr = "This is a string";

            Piranha.Extend.Fields.StringField field = inStr;
            Assert.Equal(inStr, field.Value);

            string outStr = field;
            Assert.Equal(inStr, outStr); 
        }

        [Fact]
        public void TextFieldConversions() {
            var inStr = "This is a string";

            Piranha.Extend.Fields.TextField field = inStr;
            Assert.Equal(inStr, field.Value);

            string outStr = field;
            Assert.Equal(inStr, outStr); 
        }

        [Fact]
        public void GetFieldTitleNull() {
            var field = new Piranha.Extend.Fields.TextField();

            Assert.Null(field.GetTitle());
        }

        [Fact]
        public void GetFieldTitle() {
            Piranha.Extend.Fields.TextField field = "String value";

            Assert.Equal("String value", field.GetTitle());
        }

        [Fact]
        public void GetFieldTitleMaxLength() {
            var sb = new StringBuilder();
            for (var n = 0; n < 10; n++) {
                sb.Append("NineChars");
            }

            Piranha.Extend.Fields.TextField field = sb.ToString();
            
            Assert.Equal(43, field.GetTitle().Length);
        }
    }
}
