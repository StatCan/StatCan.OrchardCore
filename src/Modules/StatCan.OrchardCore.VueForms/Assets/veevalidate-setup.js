
// register VeeValidate components globally
Vue.component('validation-provider', VeeValidate.ValidationProvider);
Vue.component('validation-observer', VeeValidate.ValidationObserver); 

// include default english and french translations.
VeeValidate.localize({
  en:{
    "code": "en",
    "messages": {
      "alpha": "The {_field_} field may only contain alphabetic characters",
      "alpha_num": "The {_field_} field may only contain alpha-numeric characters",
      "alpha_dash": "The {_field_} field may contain alpha-numeric characters as well as dashes and underscores",
      "alpha_spaces": "The {_field_} field may only contain alphabetic characters as well as spaces",
      "between": "The {_field_} field must be between {min} and {max}",
      "confirmed": "The {_field_} field confirmation does not match",
      "digits": "The {_field_} field must be numeric and exactly contain {length} digits",
      "dimensions": "The {_field_} field must be {width} pixels by {height} pixels",
      "email": "The {_field_} field must be a valid email",
      "excluded": "The {_field_} field is not a valid value",
      "ext": "The {_field_} field is not a valid file",
      "image": "The {_field_} field must be an image",
      "integer": "The {_field_} field must be an integer",
      "length": "The {_field_} field must be {length} long",
      "max_value": "The {_field_} field must be {max} or less",
      "max": "The {_field_} field may not be greater than {length} characters",
      "mimes": "The {_field_} field must have a valid file type",
      "min_value": "The {_field_} field must be {min} or more",
      "min": "The {_field_} field must be at least {length} characters",
      "numeric": "The {_field_} field may only contain numeric characters",
      "oneOf": "The {_field_} field is not a valid value",
      "regex": "The {_field_} field format is invalid",
      "required_if": "The {_field_} field is required",
      "required": "The {_field_} field is required",
      "size": "The {_field_} field size must be less than {size}KB"
    }
  },
  fr: {
    "code": "fr",
    "messages": {
      "alpha": "Le champ {_field_} ne peut contenir que des lettres",
      "alpha_num": "Le champ {_field_} ne peut contenir que des caractères alpha-numériques",
      "alpha_dash": "Le champ {_field_} ne peut contenir que des caractères alpha-numériques, tirets ou soulignés",
      "alpha_spaces": "Le champ {_field_} ne peut contenir que des lettres ou des espaces",
      "between": "Le champ {_field_} doit être compris entre {min} et {max}",
      "confirmed": "Le champ {_field_} ne correspond pas",
      "digits": "Le champ {_field_} doit être un nombre entier de {length} chiffres",
      "dimensions": "Le champ {_field_} doit avoir une taille de {width} pixels par {height} pixels",
      "email": "Le champ {_field_} doit être une adresse e-mail valide",
      "excluded": "Le champ {_field_} doit être une valeur valide",
      "ext": "Le champ {_field_} doit être un fichier valide",
      "image": "Le champ {_field_} doit être une image",
      "integer": "Le champ {_field_} doit être un entier",
      "length": "Le champ {_field_} doit contenir {length} caractères",
      "max_value": "Le champ {_field_} doit avoir une valeur de {max} ou moins",
      "max": "Le champ {_field_} ne peut pas contenir plus de {length} caractères",
      "mimes": "Le champ {_field_} doit avoir un type MIME valide",
      "min_value": "Le champ {_field_} doit avoir une valeur de {min} ou plus",
      "min": "Le champ {_field_} doit contenir au minimum {length} caractères",
      "numeric": "Le champ {_field_} ne peut contenir que des chiffres",
      "oneOf": "Le champ {_field_} doit être une valeur valide",
      "regex": "Le champ {_field_} est invalide",
      "required": "Le champ {_field_} est obligatoire",
      "required_if": "Le champ {_field_} est obligatoire lorsque {target} possède cette valeur",
      "size": "Le champ {_field_} doit avoir un poids inférieur à {size}KB"
    }
  },
});
