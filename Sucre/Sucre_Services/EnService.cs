//using MediatR;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using Sucre_DataAccess.CQS.Commands;
//using System.Net.Http.Json;
//using System.Text.RegularExpressions;

//namespace Sucre_Services
//{
//    public class EnService
//    {
//        private IConfiguration _configuration;
//        private IMediator _mediator;
//        public string Textt { get; set; }


//        public EnService(IConfiguration configuration,
//            IMediator mediator)
//        {
//            _configuration = configuration;
//            _mediator = mediator;
//            LazyInit();    
        
//        }

//        public async Task LazyInit()
//        {
//            Textt = "\n                    <div class=\"news-incut news-incut_extended news-incut_position_right news-incut_shift_top news-helpers_hide_tablet\">\n                        <div class=\"news-banner news-banner_condensed news-banner_decor\" style=\"width:300px;height:600px;\">\n    <div id=\"adfox_154589962394649315_6586c603a87b2\"></div>\n</div>\n\n<script>\n    window.yaContextCb.push(() => {\n        Ya.adfoxCode.createAdaptive({\n                ownerId: 239538,\n                containerId: 'adfox_154589962394649315_6586c603a87b2',\n                params: {\n                    p1: 'cdale',\n                    p2: 'fgou',\n                    puid26: 'people',\n                    puid28: 'belarus:vse-i-srazu-sp-betera:novyj-god:podarki:prazdniki'\n\n                }\n            },\n            ['desktop', 'tablet'],\n            {\n                tabletWidth: 1000,\n                phoneWidth: 640,\n                isAutoReloads: false\n        });\n    });\n</script>\n                    </div>\n                    <div class=\"news-header news-header_extended \">\n                        <div class=\"news-header__title\">\n                            \n                            <h1>«На Новый год друзья принесли батарею». Белорусы рассказывают истории про необычные подарки</h1>\n                        </div>\n                    </div>\n                    \n                                        <div class=\"news-entry\">\n                        <div class=\"news-entry__speech\">\n                            <p>Эту гонку не остановить. Подарки дарят в школах соседям по партам, коллегам на работе, прячут для родных и близких под праздничной елкой. Многие из них забываются и просто теряются, а вот память о некоторых остается навсегда. Вместе с <a href=\"https://pm.by/ru/pages/new-year-advent-fair?utm_source=pr_onliner_site&amp;t_group=pr&amp;utm_medium=cpc&amp;utm_content=banner&amp;cpid=52b07a12-6534-4575-bdad-99b6e80947c9&amp;utm_campaign=pf_adventny24&amp;utm_term=general_adventny24_nobonus\" target=\"_blank\">Betera</a> мы записали несколько теплых историй о необычных подарках.</p>                        </div>\n                    </div>\n                    <div id=\"adfox_157605165394762991_6586c603a8860\"></div>\n<script>\n  window.yaContextCb.push(() => {\n      Ya.adfoxCode.createScroll({\n              ownerId: 239538,\n              containerId: 'adfox_157605165394762991_6586c603a8860',\n              params: {\n                  p1: 'ccrwd',\n                  p2: 'fork',\n                  puid26: 'people',\n                  puid28: 'belarus:vse-i-srazu-sp-betera:novyj-god:podarki:prazdniki'\n              },\n              lazyLoad: {\n                  fetchMargin: 100,\n                  mobileScaling: 2\n              }\n          },\n          ['phone'],\n          {\n              tabletWidth: 1000,\n              phoneWidth: 640,\n              isAutoReloads: false\n          });\n  });\n</script>\n                    <h2>История трогательная: «Денег на куклу Barbie не было, но родители все равно купили»</h2>\n\n<div class=\"news-media news-media_extended\"><div class=\"news-media__inside\"><div class=\"news-media__viewport\"><div class=\"news-media__preview\"><img loading=\"lazy\" class=\"alignnone size-1400x5616 wp-image-1087845 news-media__image\" src=\"https://content.onliner.by/news/1400x5616/a3925a65b5b883d40463991c625cfe1e.jpg\" alt=\"\"></div>\n\n</div>\n\n</div>\n\n</div>\n\n<p>Детство Юля провела в деревне Бакшты на окраине Налибокской пущи. Родители преподавали в школе. В непростых девяностых семья жила без излишеств. Несмотря на это, во времена дефицита мама все равно сумела раздобыть для дочек по кукле Barbie, от которых тогда фанатели практически все белорусские девчонки.</p>\n\n<p><em>— В середине девяностых все у нас было достаточно скромненько,</em> — вспоминает Юлия. — <em>В 10 лет я мечтала получить куклу Barbie, но воспитывали нас не так, как это часто принято сейчас. Когда мы ходили в магазин «Детский мир», я не говорила «Мама, купи мне вот это», а спрашивала: «Мамочка, а на что у нас сегодня есть деньги?» Иногда она говорила «На вафельку», а иногда я понимала, что мы купим что-нибудь только в следующий раз… О Barbie я мечтала втайне и осознавала, что она стоит каких-то нереальных денег. Эта кукла представлялась невероятной ценностью, которая может быть только у избранных.</em></p>\n\n<blockquote><em>Ни у кого из девочек из моей деревни на тот момент ее точно не было, разве что у двоюродных сестер, живших в городе. Мы тогда играли в другие куклы, они были значительно проще.</em></blockquote>\n\n<p>Новый год родители Юли обычно отмечали с друзьями — ходили друг к другу по очереди. Вот и в тот раз семья нашей героини собиралась в гости. Выходя из дома, Юля заметила, что под елкой еще ничего не было.</p>\n\n<div class=\"news-media news-media_extended\"><div class=\"news-media__inside\"><div class=\"news-media__viewport\"><div class=\"news-media__preview\"><img loading=\"lazy\" class=\"aligncenter\" src=\"https://content.onliner.by/news/1400x5616/aadda2e74ec07745c1d8e3a8247bbb75.jpg\" alt=\"\"></div>\n\n</div>\n\n</div>\n\n</div>\n\n<p><em>— Домой мы вернулись в два часа ночи. Я обнаружила, что там уже лежат две Barbie — для меня и младшей сестры. Кудрявая блондинка ушла сестре, а брюнетка с гладкими волосами досталась мне. В то время я уже сомневалась в существовании Деда Мороза, а тут подумала, что он действительно есть.</em></p>\n\n<p><em>Через несколько лет стало известно, что мама в тот праздничный вечер просто на секундочку задержалась дома, и, когда мы вышли, она быстренько поставила заготовленные подарки под елку.</em></p>\n\n<blockquote><em>Сейчас я понимаю, что это была ненастоящая Barbie, но для меня она все равно была самой совершенной в этом мире.</em></blockquote>\n\n<p>Юля вспоминает, что несколько лет играла с куклой и специально по выкройкам шила для нее одежду. Ради нового образа пришлось даже пожертвовать бабушкиным тюлем.</p>\n\n<p><em>— Нам нужна была фата для свадебного платья, а тюль подходил для этого лучше всего. Я думала, что бабушка не заметит, но, конечно же, она потом все увидела и нас отругала. Но кукла была очень красивая, и у нее было много нарядов,</em> — подчеркивает Юля. — <em>Как часто я вспоминаю про тот случай? Практический каждый Новый год. Это история о том, что всегда стоит верить в чудо. Когда я подкладываю что-то детям под елку и вижу их радость, то вспоминаю себя: ведь и я была такой.</em></p>\n\n<p><a href=\"https://pm.by/ru/pages/new-year-advent-fair?utm_source=pr_onliner_site&amp;t_group=pr&amp;utm_medium=cpc&amp;utm_content=banner&amp;cpid=52b07a12-6534-4575-bdad-99b6e80947c9&amp;utm_campaign=pf_adventny24&amp;utm_term=general_adventny24_nobonus\" target=\"_blank\"><div class=\"news-media news-media_extended\"><div class=\"news-media__inside\"><div class=\"news-media__viewport\"><div class=\"news-media__preview\"><img loading=\"lazy\" class=\"aligncenter\" src=\"https://content.onliner.by/news/1400x5616/3fb0aae3d109a9e84ee6f4e1e6e12cbf.png\" alt=\"\"></div></div></div></div></a></p>\n\n<div class=\"news-banner news-banner_condensed news-helpers_show_tablet\">\n        <div id=\"adfox_155263550867099776_6586c603a88f9\"></div>\n    <script>\n        window.yaContextCb.push(() => {\n            Ya.adfoxCode.createAdaptive({\n                    ownerId: 260941,\n                    containerId: 'adfox_155263550867099776_6586c603a88f9',\n                    params: {\n                        p1: 'cdtqq',\n                        p2: 'gekv',\n                        puid1: 'people',\n                        puid2: 'belarus:vse-i-srazu-sp-betera:novyj-god:podarki:prazdniki',\n                        puid3: 'socium'\n                    }\n                },\n                ['phone'],\n                {\n                    tabletWidth: 1000,\n                    phoneWidth: 640,\n                    isAutoReloads: false\n                });\n        });\n    </script>\n    </div>\n\n\n";

//        }

//        public async Task Rate()
//        {
//            //1 get text
//            var text = Textt;

//            //dictionary keyword
//            var dictionary = _configuration.GetSection("dictionary")
//                .GetChildren()
//                .ToDictionary(section => section.Key, section => Convert.ToInt32(section.Value));

//            //var jj = _configuration["dictionary"];
//            //var dd = JsonConvert.DeserializeObject<Dictionary<string, int>>(jj);

//            //prepare(?) text
//            //remove html(optional)
//            var textWithoutHtml = RemoveHTMLTags(text);
//            var lemmas = await GetLemmas(textWithoutHtml);

//            int? rate = null;

//            foreach (var lemma in lemmas)
//            {   
//                if (dictionary.TryGetValue(lemma, out var value))
//                {
//                    if (rate == null)
//                    { rate = value; }
//                    else
//                    { rate += value; }
//                }
//            }

//            await _mediator.Send(
//                new SetArticleRateCommand { Id = Guid.NewGuid(), Rate = rate});

//        }

//        private string RemoveHTMLTags(string html)
//        {
//            return Regex.Replace(html, "<.*?>", string.Empty);
//        }

//        private async Task<string[]> GetLemmas(string text)
//        {
//            using (var httpClient = new HttpClient())
//            {
//                var request = new HttpRequestMessage(
//                    HttpMethod.Post,
//                    $"http://api.ispras.ru/texterra/v1/nlp?targetType=lemma&apikey={_configuration["AppSettings:IsprasKey"]}");
//                request.Headers.Add("Accept", "application/json");
//                request.Content = JsonContent.Create(new[]
//                {
//                    new { Text = text }
//                });
                    
//                var response = await httpClient.SendAsync(request);
//                if (response.IsSuccessStatusCode)
//                {
//                    var responseString = await response.Content.ReadAsStringAsync();
//                    var lemmas = JsonConvert.DeserializeObject<IsprasLemmatizationResponse[]>(responseString)?
//                        .FirstOrDefault()?
//                        .Annotations
//                        .Lemma
//                        .Select(x => x.Value)
//                        .ToArray();
//                    return lemmas;
//                }
//                return new string[] { };
//            }
//        }
//    }

    

//}
