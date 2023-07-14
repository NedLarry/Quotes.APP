using Newtonsoft.Json;
using Quotes.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Quotes.App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : CarouselPage
    {

        public static List<Quote> QUOTES { get; set; } = new List<Quote>();

        public MainPage()
        {
            InitializeComponent();
            //store content in list


            //Styles
            Resources = new ResourceDictionary
            {

                //Layouts
                {"layout",
                    new Style(typeof(StackLayout))
                    {
                        Setters =
                        {
                            new Setter
                            {
                               Property = StackLayout.HorizontalOptionsProperty,
                               Value = LayoutOptions.FillAndExpand
                            },
                            new Setter
                            {
                               Property = StackLayout.VerticalOptionsProperty,
                               Value = LayoutOptions.CenterAndExpand
                            },
                            new Setter
                            {
                               Property = StackLayout.PaddingProperty,
                               Value = new Thickness(2)
                            }
                        }
                    }
                },

                {"quoteLayout",
                    new Style(typeof(StackLayout))
                    {
                        Setters =
                        {
                            new Setter
                            {
                               Property = StackLayout.HorizontalOptionsProperty,
                               Value = LayoutOptions.FillAndExpand
                            },
                            new Setter
                            {
                               Property = StackLayout.VerticalOptionsProperty,
                               Value = LayoutOptions.CenterAndExpand
                            },
                            new Setter
                            {
                               Property = StackLayout.PaddingProperty,
                               Value = new Thickness(2)
                            },
                            new Setter
                            {
                               Property = StackLayout.HeightRequestProperty,
                               Value = 80
                            },
                            new Setter
                            {
                               Property = StackLayout.WidthRequestProperty,
                               Value = 250
                            },
                            new Setter
                            {
                               Property = StackLayout.MarginProperty,
                               Value = new Thickness(2, 30, 2, 10)
                            }
                        }
                    }
                },

                //labels
                {"quoteLabelStyle",
                    new Style(typeof(Label))
                    {
                        Setters =
                        {
                            new Setter
                            {
                               Property = Label.HorizontalOptionsProperty,
                               Value = LayoutOptions.CenterAndExpand
                            },
                            new Setter
                            {
                               Property = Label.VerticalOptionsProperty,
                               Value = LayoutOptions.CenterAndExpand
                            },
                            new Setter
                            {
                               Property = Label.VerticalTextAlignmentProperty,
                               Value = FlexJustify.SpaceAround
                            },
                            new Setter
                            {
                               Property = Label.HeightRequestProperty,
                               Value = 80
                            },
                            new Setter
                            {
                               Property = Label.WidthRequestProperty,
                               Value = 400
                            },
                            new Setter
                            {
                               Property = Label.TextColorProperty,
                               Value = Color.White
                            },
                            new Setter
                            {
                               Property = Label.FontFamilyProperty,
                               Value = "roboto"
                            },
                            new Setter
                            {
                               Property = Label.FontSizeProperty,
                               Value = 20
                            }
                            ,
                            new Setter
                            {
                               Property = Label.PaddingProperty,
                               Value = new Thickness(4)
                            }
                            ,
                            new Setter
                            {
                               Property = Label.BackgroundColorProperty,
                               Value = Color.Red
                            }
                        }
                    }
                },
                {"authorLabelStyle",
                    new Style(typeof(Label))
                    {
                        Setters =
                        {
                            new Setter
                            {
                               Property = Label.HorizontalOptionsProperty,
                               Value = LayoutOptions.Center
                            },
                            new Setter
                            {
                               Property = Label.VerticalOptionsProperty,
                               Value = LayoutOptions.Center
                            },
                            new Setter
                            {
                               Property = Label.TextColorProperty,
                               Value = Color.White
                            },
                            new Setter
                            {
                               Property = Label.FontFamilyProperty,
                               Value = "robotoItalic"
                            },
                            new Setter
                            {
                               Property = Label.FontAttributesProperty,
                               Value = FontAttributes.Italic
                            },
                            new Setter
                            {
                               Property = Label.FontSizeProperty,
                               Value = 16
                            }
                        }
                    }
                }
            };


            SetPageElements();

        }

        //http client to make api call        
        public List<Quote> GetQuotes()
        {
            try
            {
                HttpClient client = new HttpClient();

                HttpResponseMessage response = client.GetAsync("https://type.fit/api/quotes").Result;

                if (response.IsSuccessStatusCode)
                {
                    string contentAsString = response.Content.ReadAsStringAsync().Result;

                    var result = JsonConvert.DeserializeObject<List<Quote>>(contentAsString);

                    return result;
                    //return JsonConvert.DeserializeObject<List<Quote>>(contentAsString);
                }
                
                return new List<Quote>();


            }catch(Exception ex)
            {
                Console.WriteLine($"App Error: {ex.Message}");
                return new List<Quote> { };

            }
        }

        //render in content view
        public void SetPageElements()
        {
            try
            {
                List<Quote> quotes = GetQuotes();

                if (quotes.Any())
                {
                    //main Layout Page
                    foreach (Quote quote in quotes.Take(4))
                    {
                        var layout = new StackLayout()
                        {
                            Style = (Style)Resources["layout"],
                        };

                        //Layout for quote
                        var quoteLayout = new StackLayout()
                        {
                            Style = (Style)Resources["quoteLayout"]
                        };

                        //quote text label
                        quoteLayout.Children.Add(new Label
                        {
                            Style = (Style)Resources["quoteLabelStyle"],
                            Text = quote.text
                        });

                        layout.Children.Add(quoteLayout);

                        //Author label
                        layout.Children.Add(new Label
                        {
                            Style = (Style)Resources["authorLabelStyle"],
                            Text = quote.author
                        });

                        Children.Add(new ContentPage { Content = layout });
                    }

                    return;
                }

                Children.Add(new ContentPage
                {
                    Content = new Label
                    {
                        Style = (Style)Resources["quoteLabelStyle"],
                        Text = $"No internet connectivity"
                    }
                });

                return;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"App Error: {ex.Message}");
                return;
            }
        }
    }
}