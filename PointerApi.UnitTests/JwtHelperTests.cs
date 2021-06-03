//using JWT;
//using JWT.Algorithms;
//using JWT.Serializers;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using NUnit.Framework;
//using PointerApi.Authentication;
//using PointerApi.Data;
//using PointerApi.Models;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;

//namespace PointerApi.UnitTests
//{
//    public class JwtHelperTests
//    {
//        private const string _mockKid = "debb8601-a174-416f-91b3-fcdadace0f45";
//        private const string _mockS = "2bd77ba2-156e-4e7f-85fa-58f44e37c7a0";

//        private DbContextOptions<PointerContext> _dbContextOptions;

//        [OneTimeSetUp]
//        public void OneTimeSetup()
//        {
//            _dbContextOptions = new DbContextOptionsBuilder<PointerContext>()
//                .UseInMemoryDatabase(databaseName: "pointer")
//                .Options;

//            using var context = new PointerContext(_dbContextOptions);

//            context.ConsumingApplication.Add(new ConsumingApplication
//            {
//                Id = 1,
//                ApiKey = _mockKid,
//                SecretKey = _mockS,
//                ApplicationName = "test app",
//                ApplicationDescription = "test description",
//                IsDisabled = false,
//                DateEntered = DateTime.Now
//            });

//            context.SaveChanges();
//        }

//        [Test]
//        public void Should_Resolve_Signing_Key_When_Valid_Kid_In_Token()
//        {
//            using var context = new PointerContext(_dbContextOptions);

//            var jwtHelper = new JwtHelpers(context);

//            var token = new JwtSecurityToken(ValidToken(_mockKid, _mockS));

//            var keys = (List<SecurityKey>)jwtHelper.ResolveSigningKey(token.ToString(), token, _mockKid, new TokenValidationParameters());

//            Assert.AreEqual(1, keys.Count);
//        }

//        [Test]
//        public void Should_Not_Resolve_Signing_Key_When_Invalid_Kid_In_Token()
//        {
//            using var context = new PointerContext(_dbContextOptions);

//            var jwtHelper = new JwtHelpers(context);

//            var token = new JwtSecurityToken(ValidToken("ehjwehfkjwehwefkjfhefekwjh", _mockS));

//            var keys = (List<SecurityKey>)jwtHelper.ResolveSigningKey(token.ToString(), token, _mockKid, new TokenValidationParameters());

//            Assert.AreEqual(0, keys.Count);
//        }

//        [Test]
//        public void Should_Validate_Lifetime_When_Token_Is_Valid()
//        {
//            using var context = new PointerContext(_dbContextOptions);

//            var jwtHelper = new JwtHelpers(context);

//            var token = new JwtSecurityToken(ValidToken(_mockKid, _mockS));

//            var outcome = jwtHelper.LifetimeValidator(null, null, token, new TokenValidationParameters());

//            Assert.IsTrue(outcome);
//        }

//        [Test]
//        public void Should_Not_Validate_Lifetime_When_Token_Marked_As_Issued_Over_30_Seconds_Ago()
//        {
//            using var context = new PointerContext(_dbContextOptions);

//            var jwtHelper = new JwtHelpers(context);

//            var token = new JwtSecurityToken(TokenIssued60SecondsAgo(_mockKid, _mockS));

//            var outcome = jwtHelper.LifetimeValidator(null, null, token, new TokenValidationParameters());

//            Assert.IsFalse(outcome);
//        }


//        /*
//         *  Private helper methods
//         */

//        private string ValidToken(string kid, string secret)
//        {
//            var exp = UnixEpocBuilder(30); // expires 30 seconds after creation
//            var iat = UnixEpocBuilder();

//            var payload = new Dictionary<string, object>
//            {
//                { "exp", exp },
//                { "iat", iat },
//                { "kid", kid }
//            };

//            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
//            IJsonSerializer serializer = new JsonNetSerializer();
//            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
//            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

//            return encoder.Encode(payload, secret);
//        }

//        private string ExpiredToken(string kid, string secret)
//        {
//            var exp = UnixEpocBuilder(-60); // expired 60 seconds ago
//            var iat = UnixEpocBuilder();

//            var payload = new Dictionary<string, object>
//            {
//                { "exp", exp },
//                { "iat", iat },
//                { "kid", kid }
//            };

//            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
//            IJsonSerializer serializer = new JsonNetSerializer();
//            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
//            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

//            return encoder.Encode(payload, secret);
//        }

//        private string TokenIssued60SecondsAgo(string kid, string secret)
//        {
//            var exp = UnixEpocBuilder(-0);
//            var iat = UnixEpocBuilder(-60); // issued 60 seconds ago

//            var payload = new Dictionary<string, object>
//            {
//                { "exp", exp },
//                { "iat", iat },
//                { "kid", kid }
//            };

//            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
//            IJsonSerializer serializer = new JsonNetSerializer();
//            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
//            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

//            return encoder.Encode(payload, secret);
//        }

//        private string InvalidExpAndIatTokenBuilder(string kid, string secret)
//        {
//            var exp = UnixEpocBuilder(-500);
//            var iat = UnixEpocBuilder(-500);

//            var payload = new Dictionary<string, object>
//            {
//                { "exp", exp },
//                { "iat", iat },
//                { "kid", kid }
//            };

//            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
//            IJsonSerializer serializer = new JsonNetSerializer();
//            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
//            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

//            return encoder.Encode(payload, secret);
//        }

//        private double UnixEpocBuilder(double offsetSeconds = 0)
//        {
//            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
//            return Math.Round((DateTime.UtcNow.AddSeconds(offsetSeconds) - unixEpoch).TotalSeconds);
//        }

//    }
//}