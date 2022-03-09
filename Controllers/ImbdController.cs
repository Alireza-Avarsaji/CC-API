using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CC_Proj1_Alireza.Domains;
using CC_Proj1_Alireza.Dtos;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Watson.LanguageTranslator.v3;
using IBM.Watson.NaturalLanguageUnderstanding.v1;
using IBM.Watson.NaturalLanguageUnderstanding.v1.Model;
using IBM.Watson.SpeechToText.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace CC_Proj1_Alireza.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImbdController : ControllerBase
    {

        private readonly ILogger<ImbdController> _logger;
        private readonly IMongoCollection<CommentEntity> _comments;
        private readonly IMongoCollection<MovieEntity> _movies;

        public ImbdController(ILogger<ImbdController> logger)
        {

            _logger = logger;
            var client = new MongoClient("mongodb://root:avkgTtbH8WtzeIOAAqtYcq3dBf6YdmXk@7e2255d7-d7a9-4c6c-a4e6-3eb428611d8a.hsvc.ir:30119/?authSource=admin");
            var database = client.GetDatabase("imdb");
            this._comments = database.GetCollection<CommentEntity>("comment");
            this._movies = database.GetCollection<MovieEntity>("movie");
        }

        [HttpPost("AddComment")]
        public message AddComment([FromBody] CreateCommentDto comment)
        {
            byte[] voiceBytes = System.Convert.FromBase64String(comment.voice);
            IamAuthenticator authenticator = new IamAuthenticator(
                apikey: "Kaq9cRWzCjoBtPim0_KQ0QD3KphbCgOHg0k_I0Cp2SsV"
                );

            SpeechToTextService speechToText = new SpeechToTextService(authenticator);
            speechToText.SetServiceUrl("https://api.us-south.speech-to-text.watson.cloud.ibm.com/instances/72d8111b-9154-4f8d-8f81-d1dfdf039497");


            var result = speechToText.Recognize(
                audio: new System.IO.MemoryStream(voiceBytes),
                contentType: "audio/flac"
                );


            var SpeechtoVoice = JsonSerializer.Deserialize<SpeechtoVoiceDto>(result.Response);

            IamAuthenticator authenticator2 = new IamAuthenticator(
            apikey: "4UhLc5EgLpvolMJ4EU18u5UrFovCZkHqVbDWThwhrdRG"
            );
            NaturalLanguageUnderstandingService naturalLanguageUnderstanding = new NaturalLanguageUnderstandingService("2021-09-22", authenticator2);
            naturalLanguageUnderstanding.SetServiceUrl("https://api.us-south.natural-language-understanding.watson.cloud.ibm.com/instances/3ded066e-e2c3-4804-a8e0-0a6da4b7dfe9");


            var result2 = naturalLanguageUnderstanding.Analyze(
            text: SpeechtoVoice.results[0].alternatives[0].transcript,
            features: new Features()
            {
                Keywords = new KeywordsOptions()
                {
                    Sentiment = true,
                    Emotion = true,
                    Limit = 8
                }
            }
            );
            var tex = SpeechtoVoice.results[0].alternatives[0].transcript;

            var langUnderstand = JsonSerializer.Deserialize<NaturalLanguageDto>(result2.Response);
            var angers = langUnderstand.keywords.Sum(x => x.emotion.anger);
            var keywordsCount = (double)langUnderstand.keywords.Count();

            if (angers / keywordsCount > 0.3)
            {
                var res2 = new message { res = tex + " wont added" };
                return res2;
            }
            var newComment = new CommentEntity(comment.MovieId, tex);
            _comments.InsertOne(newComment);


            var res = new message { res = tex + " added successfully" };
            return res;

        }


        [HttpGet("getComments/{MovieId}/{language}")]
        public GetCommentsDto Translate([FromRoute] string MovieId, [FromRoute] string language)
        {

            var comments = _comments.Find(x => x.MovieId.Equals(MovieId)).ToList().Select(x => x.Comment).ToList();


            IamAuthenticator authenticator = new IamAuthenticator(
            apikey: "ufJA35-0DBKJ_kgEFqTbeW2mybwp0KCGRm8TRBzJbpF6"
            );
            if (language == "English")
            {

                var resEn = new GetCommentsDto
                {
                    comments = comments
                };


                return resEn;
            }
            var model = "en-es";
            if (language == "Spanish")
                model = "en-es";


            if (language == "French")
                model = "en-fr-CA";


            LanguageTranslatorService languageTranslator = new LanguageTranslatorService("2021-09-22", authenticator);
            languageTranslator.SetServiceUrl("https://api.us-south.language-translator.watson.cloud.ibm.com/instances/2a48d1a5-ce8c-4b05-8df8-878d72aedc2f");

            var result = languageTranslator.Translate(
            text: comments,
            modelId: model
            );
            var c = JsonSerializer.Deserialize<TranslateDto>(result.Response);

            Console.WriteLine(c.translations[0].translation);

            var res = new GetCommentsDto
            {
                comments = c.translations.Select(x => x.translation).ToList()
            };


            return res;

        }


        [HttpGet("getMovies")]
        public List<GetMovieDto> getMovies()
        {
            var movies = _movies.Find(x => true).ToList();
            var movies2 = movies?.Select(x => new GetMovieDto
            {
                name = x.Name,
                id = x.Id,
                url = x.Url,
                year = x.Year

            }).ToList();


            return movies2;
        }


        [HttpGet("test")]
        public string Test()
        {
            IamAuthenticator authenticator = new IamAuthenticator(
         apikey: "ufJA35-0DBKJ_kgEFqTbeW2mybwp0KCGRm8TRBzJbpF6"
         );
            LanguageTranslatorService languageTranslator = new LanguageTranslatorService("2021-09-22", authenticator);
            languageTranslator.SetServiceUrl("https://api.us-south.language-translator.watson.cloud.ibm.com/instances/2a48d1a5-ce8c-4b05-8df8-878d72aedc2f");

            var result = languageTranslator.ListLanguages();

            Console.WriteLine(result.Response);
            var m = new MovieEntity
            {
                Id = "a",
                Name = "salam"
            };
            _movies.InsertOne(m);

            return "done";
        }


    }
}
