using System.Linq;
using FluentValidation;
using NzbDrone.Core.Annotations;
using NzbDrone.Core.Languages;
using NzbDrone.Core.Validation;

namespace NzbDrone.Core.CustomFormats
{
    public class LanguageSpecificationValidator : AbstractValidator<LanguageSpecification>
    {
        public LanguageSpecificationValidator()
        {
            RuleFor(c => c.Value).Custom((value, context) =>
            {
                if (!Language.All.Any(o => o.Id == value))
                {
                    context.AddFailure(string.Format("Invalid Language condition value: {0}", value));
                }
            });
        }
    }

    public class LanguageSpecification : CustomFormatSpecificationBase
    {
        private static readonly LanguageSpecificationValidator Validator = new LanguageSpecificationValidator();

        public override int Order => 3;
        public override string ImplementationName => "Language";

        [FieldDefinition(1, Label = "CustomFormatsSpecificationLanguage", Type = FieldType.Select, SelectOptions = typeof(LanguageFieldConverter))]
        public int Value { get; set; }

        [FieldDefinition(1, Label = "CustomFormatsSpecificationExceptLanguage", HelpText = "CustomFormatsSpecificationExceptLanguageHelpText", Type = FieldType.Checkbox)]
        public bool ExceptLanguage { get; set; }

        public override bool IsSatisfiedBy(CustomFormatInput input)
        {
            if (Negate)
            {
                return IsSatisfiedByWithNegate(input);
            }

            return IsSatisfiedByWithoutNegate(input);
        }

        protected override bool IsSatisfiedByWithoutNegate(CustomFormatInput input)
        {
            var comparedLanguage = input.MovieInfo != null && input.Movie != null && Value == Language.Original.Id && input.Movie.MovieMetadata.Value.OriginalLanguage != Language.Unknown
                ? input.Movie.MovieMetadata.Value.OriginalLanguage
                : (Language)Value;

            if (ExceptLanguage)
            {
                return input.Languages?.Any(l => l != comparedLanguage) ?? false;
            }

            return input.Languages?.Contains(comparedLanguage) ?? false;
        }

        private bool IsSatisfiedByWithNegate(CustomFormatInput input)
        {
            var comparedLanguage = input.MovieInfo != null && input.Movie != null && Value == Language.Original.Id && input.Movie.MovieMetadata.Value.OriginalLanguage != Language.Unknown
                ? input.Movie.MovieMetadata.Value.OriginalLanguage
                : (Language)Value;

            if (ExceptLanguage)
            {
                return !input.Languages?.Any(l => l != comparedLanguage) ?? false;
            }

            return !input.Languages?.Contains(comparedLanguage) ?? false;
        }

        public override NzbDroneValidationResult Validate()
        {
            return new NzbDroneValidationResult(Validator.Validate(this));
        }
    }
}
