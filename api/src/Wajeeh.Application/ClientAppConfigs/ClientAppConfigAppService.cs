using Abp.Application.Services;
using Abp.Domain.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.Serialization;
using System.Text;
using Wajeeh.ClientAppAbouts;
using Wajeeh.ClientAppConfigs.Dto;
using Wajeeh.ClientAppTermsAndPolicies;

namespace Wajeeh.ClientAppConfigs
{
    public class ClientAppConfigAppService : ApplicationService, IClientAppConfigAppService
    {
        private readonly IRepository<ClientAppTermsAndPolicy, long> _termsAndPolicyRepository;
        private readonly IRepository<ClientAppAbout, long> _aboutRepository;
        public ClientAppConfigAppService(IRepository<ClientAppTermsAndPolicy, long> termsAndPolicyRepository,
            IRepository<ClientAppAbout, long> aboutRepository)
        {
            _termsAndPolicyRepository = termsAndPolicyRepository;
            _aboutRepository = aboutRepository;
        }
        public TermsAndPolicyDto GetTermsAndPolicy()
        {
            var terms = _termsAndPolicyRepository.GetAll().FirstOrDefault();

            if (terms == null)
            {
                return new TermsAndPolicyDto()
                {
                    ContentText = ""
                };
            }
            else
            {

                if (CultureInfo.CurrentCulture.Name == "ar-EG")
                {
                    return new TermsAndPolicyDto()
                    {
                        ContentText = terms.ContentAr

                    };
                }
                else
                {
                    return new TermsAndPolicyDto()
                    {
                        ContentText = terms.Content

                    };
                }
            }
        }

        public TermsAndPolicyDto SetTermsAndPolicy(CreateTermsAndPolicyDto input)
        {
            var term = _termsAndPolicyRepository.GetAll().FirstOrDefault();
            if (term == null)
            {
                _termsAndPolicyRepository.Insert(new ClientAppTermsAndPolicy()
                {
                    Content = input.Content,
                    ContentAr=input.ContentAr
                });
            }
            else
            {
                term.Content = input.Content;
                term.ContentAr = input.ContentAr;
                _termsAndPolicyRepository.Update(term);
            }
            CurrentUnitOfWork.SaveChanges();

            if (CultureInfo.CurrentCulture.Name == "ar-EG")
            {
                return new TermsAndPolicyDto()
                {
                    ContentText = input.ContentAr

                };
            }
            else
            {
                return new TermsAndPolicyDto()
                {
                    ContentText = input.Content

                };
            }
        }
        public AboutDto GetAbout()
        {
            var about = _aboutRepository.GetAll().FirstOrDefault();
            if (about == null)
            {
                return new AboutDto()
                {
                    ContentText = ""
                };
            }
            else
            {
                if (CultureInfo.CurrentCulture.Name == "ar-EG")
                {
                    return new AboutDto()
                    {
                        ContentText = about.ContentAr

                    };
                }
                else
                {
                    return new AboutDto()
                    {
                        ContentText = about.Content

                    };
                }
            }
        }
        public AboutDto SetAbout(CreateAboutDto input)
        {
            var about = _aboutRepository.GetAll().FirstOrDefault();
            if (about == null)
            {
                _aboutRepository.Insert(new ClientAppAbout()
                {
                    Content = input.Content,
                    ContentAr = input.ContentAr
                });
            }
            else
            {
                about.Content = input.Content;
                about.ContentAr = input.ContentAr;
                _aboutRepository.Update(about);
            }
            CurrentUnitOfWork.SaveChanges();

            if (CultureInfo.CurrentCulture.Name == "ar-EG")
            {
                return new AboutDto()
                {
                    ContentText = input.ContentAr

                };
            }
            else
            {
                return new AboutDto()
                {
                    ContentText = input.Content

                };
            }

        }
    }
}
