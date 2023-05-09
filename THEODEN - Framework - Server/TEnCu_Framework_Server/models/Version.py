from django.db import models


class Version(models.Model):
    name = models.CharField(max_length=100)
    version = models.IntegerField()
